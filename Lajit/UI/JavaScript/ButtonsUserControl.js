//Purpose: To do all btnsUC operations in the client-side itself. Created by Shanti.

//The SetCurrentAction function will set the hidden current action value 
//and call the function InvokeOnButtonClick() which will enable/color/setdefault control based on the mode
function SetCurrentAction(mode)
{
    var hdnSubmitstatus = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnSubmitstatus');
    hdnSubmitstatus.value="";

    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    hdnCurrAction.value=mode;
    if(hdnCurrAction)
        OnButtonClick(mode);
        
    var lblMessage = document.getElementById('ctl00_cphPageContents_lblmsg');
    lblMessage.style.display = 'none';
             
           // Child grid with ispaging modify case displaying lbl message added on 171109
           /* if((typeof IsPaging=='function')&&IsPaging('ctl00_cphPageContents_CGVUC'))
            {
                if(mode=="Modify")
                {
                  lblMessage.style.display = 'Block';
                  lblMessage.innerHTML= 'Please save before clicking paging links.';
                }
            }*/
            
            
}

//...................................................................................................//

//Event handler for all btnsuc click events.
function OnButtonClick(mode)
{
    //var themeSession = document.getElementById('ctl00_hdnThemeName').value;
   // var imagesCDNPath = document.getElementById('ctl00_hdnImagesCDNPath').value;
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    
    //Getting the page controls collection.
    var arrControlAttributeColl = new Array();
    arrControlAttributeColl=GenerateControls();
    var lblMessage = document.getElementById('ctl00_cphPageContents_lblmsg');
    var hdnSubmitstatus = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnSubmitstatus');

    ShowNavRwBtns(mode);
    switch (mode)
    {
        case "Add":
        case "Clone":
        {
            //Initializing branch xml
            var hdnBranchXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML');
            hdnBranchXML.value="";

            //set timer
            var hdnTimerValue = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnTimerValue');
            if(hdnTimerValue.value=="Set")
            {
               var timer = document.getElementById('ctl00_cphPageContents_timerEntryForm');
               timer.disabled=false;
            }

            //Operations
            SetSubmitURL(g_cdnImagesPath); 
            CollapseGrid();
            OpenEntryForm();
            ChangeControlColor(arrControlAttributeColl,'#FFFFFF');
            if(mode!="Clone")
            {
                //Should not clear the controls for error case. Remaining flow is same.
                if(hdnSubmitstatus.value!="Error")
                {
                    ClearControls(arrControlAttributeColl);
                    SetCntrlDefault(arrControlAttributeColl);
                    
                    ResetChildGridAmount();
                }
            }
            if(hdnSubmitstatus.value!="Error") {
                SetCGDefaults(arrControlAttributeColl);
            }
            DisableControls(arrControlAttributeColl, false, mode);
//            ChangeControlColor(arrControlAttributeColl,'#FFFFFF');
            HideControls(arrControlAttributeColl, mode);
            ShowHideRegularExpressionValidator(arrControlAttributeColl, mode);
            HandleChildGridOPLinks(mode);
            ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath);
            ResetActionButtons(g_cdnImagesPath, mode);
            
            
            if((typeof IsPaging=='function')&& IsPaging('ctl00_cphPageContents_CGVUC')&&g_IsPostBack)
            {
                //Set the page index of the gridview to 1 in the server.
                //__doPostBack('ctl00$cphPageContents$CGVUC$grdVwBranch','Page$First');
                var hrefToCall=jQuery('.ChildGridPager a:contains(1)').attr('href');
                if(hrefToCall)
                {
                    $get('ctl00_cphPageContents_CGVUC_hdnRowsToDisplay').value='10';
                    setTimeout(hrefToCall,0);
                }
                
                g_IsPostBack=false;
                setTimeout('HideProgress()',0);
                //Hide the pager template.
                var objGrid = document.getElementById('ctl00_cphPageContents_CGVUC_grdVwBranch');
                if(objGrid)
                {
                    for (var rowCount=1;rowCount<objGrid.rows.length;rowCount++)
                    {
                        var trCurrent=objGrid.rows[rowCount];
                        var trID=trCurrent.getAttribute('id');
                        var trClass=trCurrent.className;
                        if(trClass&&trClass=='ChildGridPager')
                        {
                            trCurrent.style.display='none';
                        }
                    }
                }        
            }
          
           //Set focus of cursor at first control
           //SetFocusFirstItem();
           setTimeout('SetFocusFirstItem()',100);
           //  setTimeout(function(){jQuery("input:visible:enabled:first").focus()},50);
           //setTimeout('Test()',100);
             
                
           //Add onblur event for parent last element and Set focus child grid first element  
//           SetTabFocusChildGrid();   
            
           //Lastname querystring for vendorcommerical.aspx and customer.aspx pages
            var qsLname = queryString("LName"); 
            if(qsLname)
            {
             var tboxselecter='div[id^="ctl00_cphPageContents_pnlEntryForm"] input:text[AddLastname]';
             var tboxautofill=jQuery(tboxselecter);//.eq(0);
             //txtid.css("border","3px solid red");
             qsLname=unescape(qsLname);
             tboxautofill.val(qsLname);
            }
             
           //For menu opening popupcase. Cancel operation->normal pg redirection, so clear action after cancel.           
           //hdnCurrAction.value="";
        }
        break;
        case "Modify":
        {        
            //Operations
            SetSubmitURL(g_cdnImagesPath);
            CollapseGrid();
            OpenEntryForm();
            SetSelectedItem(arrControlAttributeColl);
            DisableControls(arrControlAttributeColl,false,"Modify");
            ChangeControlColor(arrControlAttributeColl,'#FFFFFF');
            ShowHideRegularExpressionValidator(arrControlAttributeColl, mode);
            HandleChildGridOPLinks(mode);
            ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath);
            ResetActionButtons(g_cdnImagesPath, mode);
            SetCGDefaults(arrControlAttributeColl);
            //Set focus of cursor at first control
            //SetFocusFirstItem();
            setTimeout('SetFocusFirstItem()',100);
            // setTimeout(function(){jQuery("input:visible:enabled:first").focus()},50);
            
            //Add onblur event for parent last element and Set focus child grid first element  
//            SetTabFocusChildGrid();   
            
            //ON CLICK OF LINKS
           /* if((typeof IsPaging=='function')&&IsPaging('ctl00_cphPageContents_CGVUC'))
            {
                  lblMessage.style.display = 'Block';
                  lblMessage.innerHTML= 'Please save before clicking paging links.';
            }*/
            
            //HIDE LINKS
            
            if((typeof IsPaging=='function')&& IsPaging('ctl00_cphPageContents_CGVUC')&&g_IsPostBack)
            {
                //Hide the pager template.
                var objGrid = document.getElementById('ctl00_cphPageContents_CGVUC_grdVwBranch');
                if(objGrid)
                {
                    for (var rowCount=1;rowCount<objGrid.rows.length;rowCount++)
                    {
                        var trCurrent=objGrid.rows[rowCount];
                        var trID=trCurrent.getAttribute('id');
                        var trClass=trCurrent.className;
                        if(trClass&&trClass=='ChildGridPager')
                        {
                            trCurrent.style.display='none';
                        }
                    }
                }        
            }
            
            
            
            
            ////Calling exclusive page method in pagelevel(present in AccountingLayoutItems.aspx.cs)
            //MethodInfo minfos = this.Page.GetType().GetMethod("PageExclusive");
            // if (minfos != null)
            //{
            //   minfos.Invoke(this.Page, new object[] { null });
            //}
        }
        break;
        case "Delete":
        {
            //Operations
            SetSubmitURL(g_cdnImagesPath);
            CollapseGrid();
            OpenEntryForm();
            DisableControls(arrControlAttributeColl,true,"Delete");
            HandleChildGridOPLinks(mode);
            ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath);
            ResetActionButtons(g_cdnImagesPath, mode);
        }
        break;
        case "Find":
        {
            //Initializing branch xml
            var hdnBranchXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnBranchXML');
            hdnBranchXML.value=""; 
            
            SetSubmitURL(g_cdnImagesPath);
            CollapseGrid();
            OpenEntryForm();
            //Should not clear the controls for error case. Remaining flow is same.
            if(hdnSubmitstatus.value!="Error")
            {
                ClearControls(arrControlAttributeColl);
                SetCntrlDefault(arrControlAttributeColl);
                SetCGDefaults(arrControlAttributeColl);
            }
            DisableControls(arrControlAttributeColl,false,"Find");
            ChangeControlColor(arrControlAttributeColl,'#fafad2');//LightGoldenrodYellow
            HideControls(arrControlAttributeColl, "Find");
            ShowHideRegularExpressionValidator(arrControlAttributeColl, mode);
            HandleChildGridOPLinks(mode);
            ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath);
            ResetActionButtons(g_cdnImagesPath, mode);
          
            //Set focus of cursor at first control
            setTimeout('SetFocusFirstItem()',100);
          
            //Add onblur event for parent last element and Set focus child grid first element  
//            SetTabFocusChildGrid();   
            
        }
        break;
        case "Select":
        {
            var currentPage = GetPage(window);
            if(currentPage.indexOf('JOURNALSHYB.ASPX') == -1) {//Dont show the entryform in this case as entryform does'nt make sense(bcoz of dynamic nature) in this case.
                CollapseGrid();
                OpenEntryForm();
            }
            if(currentPage.indexOf('FULLVIEW.ASPX') == -1)//other than fullview.aspx
            {
                DisableControls(arrControlAttributeColl,true, mode);
                ChangeControlColor(arrControlAttributeColl,'#d3d3d3');
                HandleChildGridOPLinks(mode);
                ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath);
                ResetActionButtons(g_cdnImagesPath, mode);
            }
        }
        break;
        case "Submit":
        {
            if(hdnCurrAction.value=="Add" || hdnCurrAction.value=="Modify" ||hdnCurrAction.value=="Clone")
                OnButtonClick("Select");
            else if(hdnCurrAction.value=="Delete" ||hdnCurrAction.value=="Find")
            {
                BlockGrid();
                ExpandGrid();
                CloseEntryForm();
                ResetActionButtons(g_cdnImagesPath, "PageLoad");
            }
            if(typeof ResetBlinkObjects=='function')//Because not all pages are referencing ChildGridView.js            
            {
                ResetBlinkObjects();
            }
            //This can be used in menu opening popups case. After successful Submit, any page should be redirected normally.(21/04/09)
            if(hdnSubmitstatus.value!="Error")
                hdnCurrAction.value=""; 
        }
        break;
        case "PageLoad":
        {
            
            if(document.getElementById('ctl00_cphPageContents_pnlGVContent')==null)//cancel in norows case(22/04/09)
            {
                lblMessage.innerHTML="";//Not to display the lblmsg while cancelling the operation.
                OnButtonClick("Select");
            }
            else
            {
                BlockGrid();
                ExpandGrid();
                CloseEntryForm();
                ResetActionButtons(g_cdnImagesPath, mode);
            }
           //For menu opening popupcase. Cancel operation->normal pg redirection, so clear action after cancel.           
            hdnCurrAction.value="";

            if(typeof ResetBlinkObjects=='function')//Because not all pages are referencing ChildGridView.js            
                ResetBlinkObjects();//Function found in ChildGridView.js
                
            //Reset the hdnModRows upon Cancel click
            var hdnModRows=$get('ctl00_cphPageContents_CGVUC_hdnModRows');
            if(hdnModRows)
            {
                $get('ctl00_cphPageContents_CGVUC_hdnModRows').value='';
            }
            
            var hdnNeedToConfirmExit = $get('ctl00_cphPageContents_BtnsUC_hdnNeedToConfirmExit');
            if(hdnNeedToConfirmExit) {
                hdnNeedToConfirmExit.value = "False";
            }

        }
        break;
    }
}

//...................................................................................................//

function ShowNavRwBtns(mode)
{
    //NavigateRows buttons should not be displayed.
    var imgbtnNext = document.getElementById('ctl00_cphPageContents_imgbtnNext');
    var imgbtnPrevious = document.getElementById('ctl00_cphPageContents_imgbtnPrevious');
    if(imgbtnNext)
    {
        if(mode=="Select")
            imgbtnNext.style.display = 'Block';
        else
            imgbtnNext.style.display = 'none';
    }
    if(imgbtnPrevious)
    { 
        if(mode=="Select")
            imgbtnPrevious.style.display = 'Block';
        else
            imgbtnPrevious.style.display = 'none';
    }
}

//...................................................................................................//

//It Handles OpLinks/tblBPCLinks/tblAmounts below the child grid. OpLinks/tblBPCLinks should be mutually exclusive always.
function HandleChildGridOPLinks(mode)
{
    var tblOperations = document.getElementById('ctl00_cphPageContents_CGVUC_tblOpLinks');
    var tblBPCLinks = document.getElementById('ctl00_cphPageContents_CGVUC_tblBPCLinks');
    var tblAmounts = document.getElementById('ctl00_cphPageContents_CGVUC_tblAmounts');
    if(mode=="Add" || mode=="Clone" || mode=="Modify")
    {
         if(tblOperations)
            tblOperations.style.display = 'Block';
         if(tblAmounts)
            tblAmounts.style.display = 'Block';
         if(tblBPCLinks)
         {
            tblBPCLinks.style.display = 'none';
            var tblRw=tblBPCLinks.rows[0];
            if(tblRw.cells.length>0)
               tblRw.cells[tblRw.cells.length-1].style.display='none';//hiding the | seperator
         }
    }
    else if(mode=="Delete" || mode=="Find" || mode=="Select")
    {
         if(tblOperations)
            tblOperations.style.display = 'none';
         if(tblAmounts)
            tblAmounts.style.display = 'none';
         if(tblBPCLinks)
         {
            tblBPCLinks.style.display = 'none';
            if(mode=="Select")
            {
                tblBPCLinks.style.display = 'Block';
            }
            var tblRw=tblBPCLinks.rows[0];
            if(tblRw.cells.length>0)
            {
               tblRw.cells[tblRw.cells.length-1].style.display='none';//hiding the | seperator
            }
         }
   }
   //ArDescription branch exclusively for ARinvDetail.aspx page
   if (document.getElementById('ctl00_cphPageContents_CGVUCArdescription_tblOpLinks') != null) {
       HandleChildGridArDescriptionOPLinks(mode);
   }
}

//...................................................................................................//
//It Handles OpLinks/tblBPCLinks/tblAmounts below the arDescription child grid.
function HandleChildGridArDescriptionOPLinks(mode) {
    var tblOperations = document.getElementById('ctl00_cphPageContents_CGVUCArdescription_tblOpLinks');
    var tblBPCLinks = document.getElementById('ctl00_cphPageContents_CGVUCArdescription_tblBPCLinks');
    var tblAmounts = document.getElementById('ctl00_cphPageContents_CGVUCArdescription_tblAmounts');
    if (mode == "Add" || mode == "Clone" || mode == "Modify") {
        if (tblOperations)
            tblOperations.style.display = 'Block';
        if (tblAmounts)
            tblAmounts.style.display = 'Block';
        if (tblBPCLinks) {
            tblBPCLinks.style.display = 'none';
            var tblRw = tblBPCLinks.rows[0];
            if (tblRw.cells.length > 0)
                tblRw.cells[tblRw.cells.length - 1].style.display = 'none'; //hiding the | seperator
        }
    }
    else if (mode == "Delete" || mode == "Find" || mode == "Select") {
        if (tblOperations)
            tblOperations.style.display = 'none';
        if (tblAmounts)
            tblAmounts.style.display = 'none';
        if (tblBPCLinks) {
            tblBPCLinks.style.display = 'none';
            if (mode == "Select") {
                tblBPCLinks.style.display = 'Block';
            }
            var tblRw = tblBPCLinks.rows[0];
            if (tblRw.cells.length > 0) {
                tblRw.cells[tblRw.cells.length - 1].style.display = 'none'; //hiding the | seperator
            }
        }
    }
}

//...................................................................................................//

//Generate an array of control cntrlId collection along with attributes.
function GenerateControls()
{
   //This would store the parent and child controls along with the columns
   var controlCollection = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnParentColNode').value;   
   
   // This would store an array of Parent and Branches along with the column attributes.
   var arrParentChild =controlCollection.split(">");

   // This would store an array of control ids along with the attributes.
   var arrControlAttributeColl = new Array( );
   var cntr="0";
   for(i=0; i<arrParentChild.length; i++) //ArrayParentChild
   {        
     // This array size is always two. The first element stores the Node name and control type. Teh second element stores the corresponsing column info for that particular node.
     var arrNodeControlColmuns=arrParentChild[i].split(";");
	//This array will store the Parent ,branch and child name along with the Control Type
     var NodeControl = arrNodeControlColmuns[0].split("/");
     
     var currBranchNodeName = NodeControl[0]; //This stores the Node name
     var brnchCtrlType = NodeControl [1]; //control type
         
     var arrCurrNodeCols=arrNodeControlColmuns[1];
     
     var objGrid=document.getElementById('ctl00_cphPageContents_CGVUC_grdVwBranch');
     var rowCount=1;
     if (objGrid && brnchCtrlType=="GView")
     {
       rowCount =objGrid.rows.length;
     }

      // Ardescription childgrid used at ARinDetail.aspx
       if (currBranchNodeName == "Ardescription") {
           var objArGrid = document.getElementById('ctl00_cphPageContents_CGVUC' + currBranchNodeName + '_grdVwBranch');
           if (objArGrid && brnchCtrlType == "GView") {
               rowCount = objArGrid.rows.length;
           }
       }

      

     // This array would store the list of controls as well as their attributes for each nodeof the OUT xml.
     var arrCols=arrCurrNodeCols.split(":");
      
      for(j=0; j<arrCols.length-1; j++)//currentnodecolumns
      {
            var arrColAttr = arrCols[j].split("|~|");
            for (k=0;k<rowCount;k++)
            {
                //Column attributes
                var attrLabel=  arrColAttr[0];
                var attrCntlType=arrColAttr[1];
                var cntrlID="";
                var trCntrlID='ctl00_cphPageContents_'+'tr'+attrLabel;
                var regCntrlID='ctl00_cphPageContents_'+'reg'+attrLabel;
                
                switch (attrCntlType)
                {
                       case "TBox":
                       case "Cal":
                       case "Calc":
                       case "Amount":
                       case "Passwd":
                       case "Phone":
                       {
                            cntrlID="txt"+attrLabel;
                       }
                       break;
                       case "DDL":
                       case "EDDL":
                       {
                            cntrlID="ddl"+attrLabel; 
                       }
                       break;
                       case "Check":
                       {
                          if(brnchCtrlType=="GView")
                          {
                            cntrlID="chkBx"+attrLabel;
                          }
                          else
                          {
                            cntrlID="chk"+attrLabel;
                          }
                       }
                       break; 
                       case "lnkbtn":
                       {
                          cntrlID="lnkbtn"+attrLabel;
                       } 
                       break; 
                       case "LBox":
                       {
                       cntrlID="lstBx"+attrLabel;
                       }
                       break;
                }

                if (currBranchNodeName !="" && currBranchNodeName !=null && (brnchCtrlType!="GView" ||brnchCtrlType==""))
                {
                   if (attrCntlType != "LBox")
                   {
                        cntrlID = cntrlID+"_"+ currBranchNodeName;
                   }
                   trCntrlID = trCntrlID+"_"+ currBranchNodeName;
                   regCntrlID = regCntrlID+"_"+ currBranchNodeName;
                }
                else if (currBranchNodeName !="" && currBranchNodeName !=null && brnchCtrlType=="GView") {

                    if (currBranchNodeName == "Ardescription") {
                        // Ardescription childgrid used at ARinDetail.aspx
                        cntrlID = 'CGVUC' + currBranchNodeName + '_grdVwBranch_GVRow' + k + "_" + cntrlID;
                        trCntrlID = 'ctl00_cphPageContents_CGVUC' + currBranchNodeName + '_grdVwBranch_GVRow' + k;
                    }
                    else {
                        cntrlID = 'CGVUC_grdVwBranch_GVRow' + k + "_" + cntrlID;
                        trCntrlID = 'ctl00_cphPageContents_CGVUC_grdVwBranch_GVRow' + k;
                    }
                }
                 
                 if (cntrlID!="")
                 {
                    //cntrlID--> This stores the client control id generated in the client side.
                    //arrColAttr[2]-->IsDefault Attribute
                    //arrColAttr[3]-->IsDisplayOnly Attribute, values 0/1
                    //arrColAttr[4]-->IsRequired Attribute, values 0/1
                    //arrColAttr[5]-->IsNumeric Attribute, values 0/1
                    //arrColAttr[6]-->IsHidden Attribute, values 0/1
                    //arrColAttr[7]-->IsSearched Attribute, values 0/1
                    //arrColAttr[1]-->Control Type Attribute
                    //trCntrlID--> This stores the client control id of the row containing cntrlID.
                    //regCntrlID--> This stores the client control id the regular expression validator of cntrlID.
                    //arrColAttr[0]-->Label Attribute
                    
                    //Autofill Overlay functionality
                    var CGAutoFill=_get('ctl00_cphPageContents_'+cntrlID)
                    var CGAfAtt;
                    if(CGAutoFill==null){ 
                        CGAfAtt='0'; 
                    }
                    else {
                        CGAfAtt=CGAutoFill.getAttribute('AutoFill')||'0';
                        if(trim(CGAfAtt.toString()).length==0) {
                            CGAfAtt='0';
                        }
                    }
                    if(!jQuery('#'+cntrlID).attr('AutoFill'))// && CGAfAtt!='1')
                    {
                        cntrlID='ctl00_cphPageContents_'+cntrlID;
                    }

                    arrControlAttributeColl[cntr]= cntrlID+","+arrColAttr[2]+","+arrColAttr[3]+","+arrColAttr[4]+","+arrColAttr[5]+","+ arrColAttr[6]+","+arrColAttr[7]+","+arrColAttr[1]+","+trCntrlID+","+regCntrlID+","+arrColAttr[0];
                    cntr++;
                 }
          }

    }//for currentnodecolumns
  } //for ArrayParentChild
  
    //arrControlAttributeColl[0]--> This stores the client control id generated in the client side.
    //arrControlAttributeColl[1]-->IsDefault Attribute
    //arrControlAttributeColl[2]-->IsDisplayOnly Attribute, values 0/1
    //arrControlAttributeColl[3]-->IsRequired Attribute, values 0/1
    //arrControlAttributeColl[4]-->IsNumeric Attribute, values 0/1
    //arrControlAttributeColl[5]-->IsHidden Attribute, values 0/1
    //arrControlAttributeColl[6]-->IsSearched Attribute, values 0/1
    //arrControlAttributeColl[7]-->Control Type Attribute
    //arrControlAttributeColl[8]--> This stores the client control id of the row containing cntrlID.
    //arrControlAttributeColl[9]--> This stores the client control id of the regular expression validator of cntrlID.
    //arrControlAttributeColl[10]--> Label Attribute
    return  arrControlAttributeColl;
}

//...................................................................................................//

// This function can be called to neable and disable UI. Status=true means you would be disabling the controls
//Mode->It says in which mode you are in ADD,FIND,MODIFY....
function DisableControls(arrControlAttributeColl, Status, mode)
{
    //Added by Danny Bug No:210
    var isSelectInvoice=false;
//    var currentPage = window.location + "";
//    if(currentPage.toUpperCase().indexOf('SELECTINVOICE.ASPX') != -1)//For SelectInvoice.aspx page
//    {
//        isSelectInvoice=true;
//        //Hide the header cell of the checkbox select column explicitly.
//        var objHdrCell=jQuery('tr.GridViewChildHeader').children('[scope]').eq(0);
//        if(Status){
//            //Hide it
//            objHdrCell.css('display','none');
//        }
//        else{
//            objHdrCell.css('display','');
//        }
//    }
    
  
    
    var arrcurrentCol=new Array();
    //var FocusFirstItem=true;
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
        var currentCol=arrControlAttributeColl[i];
        arrcurrentCol= currentCol.split(",");
        var objCurrentCntrl=document.getElementById(arrcurrentCol[0]);
        if (objCurrentCntrl)
        {
            var a=(arrcurrentCol[7]=="Check")?true:false;
            var b=(arrcurrentCol[0].indexOf('GVRow')!=-1)?true:false;
            var c=(objCurrentCntrl.parentNode.getAttribute('rowindex'))?true:false;
               if(Status==true)
               {
                    if(isSelectInvoice && a && b && c) {
                        //Purpose:To hide the select checkbox column in SelectInvoice rather than simply disabling it.
                        //Check whether the control:
                        //1.is a checkbox -a
                        //2.Belongs to the child grid -b
                        //3.Belongs to the first visible column i.e select column. -c
                        jQuery(objCurrentCntrl).closest('td').css('display','none');//Hide the parent cell.
                    }
                    else {
                        objCurrentCntrl.disabled=true;
                    }
               }
               else
               {
                   objCurrentCntrl.disabled=false;
                   objCurrentCntrl.title="";
                   //Exclusively for checkbox control but let it be happen for all controls
                   if(document.all)//IE
                   {
                        objCurrentCntrl.offsetParent.childNodes[0].disabled=false;
                        objCurrentCntrl.offsetParent.disabled=false;
                   }
                   //Set focus on first item.
                   /*if(FocusFirstItem)
                   {
                      objCurrentCntrl.focus();
                     // objCurrentCntrl.value = 'Username';
                      //objCurrentCntrl.Focus();
                      FocusFirstItem=false;
                   }*/
                   
                   //To enable all controls in every row of Child gridview. But let it be happen for all controls.
                   var trCurrent = document.getElementById(arrcurrentCol[8]);
                   if(trCurrent)
                   {
                        for(var cellCount = 0; cellCount < trCurrent.cells.length ; cellCount++)
                        {
                            trCurrent.cells[cellCount].disabled=false;
                        }
                   } 

                   if(arrcurrentCol[2]=="1")//Is Display Only
                    {
                      objCurrentCntrl.disabled=true;
                      objCurrentCntrl.title  = "You can't edit this value because it is ReadOnly";//not working in FF
                      if(document.all)//IE
                      {
                            objCurrentCntrl.offsetParent.childNodes[0].disabled=true;
                            objCurrentCntrl.offsetParent.disabled=true;
                      }
                    } 
                    //making ctrl as disabled if TrixID is empty
                    if(arrcurrentCol[7]=="DDL" ||arrcurrentCol[7]=="EDDL")
                    {
                        var newTrxIDType = trim(objCurrentCntrl.value);
                        if (newTrxIDType.Length == 0)
                        {
                            objCurrentCntrl.disabled=true;
                        }
                    }

                   switch (mode)
                   {
                          case "Add":
                          case "Clone":
                          {  
                                if(arrcurrentCol[7]=="Check")//control type
                                {
                                    if (arrcurrentCol[3] =="1")// Is Required Attribute
                                    {
                                        objCurrentCntrl.disabled=true;
                                        objCurrentCntrl.title = "You can't uncheck this checkbox because it is a required field.";
                                        if(document.all)//IE
                                        {
                                            objCurrentCntrl.offsetParent.childNodes[0].disabled=true;
                                            objCurrentCntrl.offsetParent.disabled=true;
                                        } 
                                        objCurrentCntrl.checked=true; 
                                    }
                                } 
                           }
                           break;
                           case "Modify":
                           {
                                if(arrcurrentCol[7]=="Check")
                                {
                                    if (arrcurrentCol[3] =="1")// Is Required Attribute
                                    {
                                        objCurrentCntrl.disabled=true;
                                        objCurrentCntrl.title = "You can't uncheck this checkbox because it is a required field.";
                                        if(document.all)//IE
                                        {
                                            objCurrentCntrl.offsetParent.childNodes[0].disabled=true;
                                            objCurrentCntrl.offsetParent.disabled=true;
                                        } 
                                        objCurrentCntrl.checked=true; 
                                    }
                                    //Show em back
                                    if(isSelectInvoice && b && c) {
                                        jQuery(objCurrentCntrl).closest('td').css('display','');//Hide the parent cell.
                                    }
                                }
                           }
                           break;
                           case "Delete":
                           break;
                           case "Find":
                           {
                                if (arrcurrentCol[6]==0)//IsSearched
                                {
                                    objCurrentCntrl.disabled=true;
                                    objCurrentCntrl.title = "You can't edit this value because it is not a searchable criteria";
                                    if(document.all)//IE
                                    {
                                        objCurrentCntrl.offsetParent.childNodes[0].disabled=true;
                                        objCurrentCntrl.offsetParent.disabled=true;
                                    }
                                }
                                else if(arrcurrentCol[6]==1)// This piece of the code is to override the IsDisplay=1 property when IsSearched=1.
                                {
                                    objCurrentCntrl.disabled=false;
                                    if(document.all)//IE
                                    {
                                        objCurrentCntrl.offsetParent.childNodes[0].disabled=false;
                                        objCurrentCntrl.offsetParent.disabled=false;
                                    }
                                }
                           }
                           break;
                           default:
                           {
                                objCurrentCntrl.disabled=true;
                                if(document.all)//IE
                                {
                                    objCurrentCntrl.offsetParent.childNodes[0].disabled=true;
                                    objCurrentCntrl.offsetParent.disabled=true;
                                }
                           }
                           break;
                       }//switch ends
               }
       }
    }// For Loops Ends

    //Only For HelpAuthor page.
    var currentPage = window.location + "";//Cast the object to string.
    if(currentPage.toUpperCase().indexOf('HELPAUTHOR.ASPX') != -1)//For HelpAuthor.aspx page
    {
        var objtxtFileAttachment = document.getElementById('ctl00_cphPageContents_txtFileAttachment');
        var objtxtHelpFile = document.getElementById('ctl00_cphPageContents_txtHelpFile');
      
        if(Status==true)
        {
            objtxtFileAttachment.style.display = 'none';
            objtxtHelpFile.style.display = 'Block';
            objtxtHelpFile.disabled=true;
        }
        else
        {
            objtxtFileAttachment.style.display = 'Block';
            objtxtHelpFile.style.display = 'none';
            objtxtFileAttachment.disabled=false;
        }
        
        var ddlHelpFileType=document.getElementById('ctl00_cphPageContents_ddlHelpFileType');
        var ddlSelText=ddlHelpFileType.options[ddlHelpFileType.selectedIndex].text;
        if(ddlSelText && ddlSelText.toUpperCase()=="URL")
        {
            objtxtFileAttachment.style.display = 'none';
            objtxtHelpFile.style.display = '';
            objtxtHelpFile.disabled=false;
        }
    }
}//DisableEnableui --ends

//...................................................................................................//

// This function can be called to change the background color of the controls.
function ChangeControlColor(arrControlAttributeColl, colorCode)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
       var currentCol=arrControlAttributeColl[i];
       arrcurrentCol= currentCol.split(",");
       var objCurrentCntrl=  document.getElementById(arrcurrentCol[0]);
       if (objCurrentCntrl)
       {
              if (arrcurrentCol[7]=="Lbl"||arrcurrentCol[7]=="Check")
                objCurrentCntrl.style.backgroundColor = 'transparent';
              else
                objCurrentCntrl.style.backgroundColor = colorCode;
       }
    }// For Loops Ends
    
    //Change the Icon present within the textbox's colour.
    jQuery('div.txtIconLeft').css('backgroundColor',colorCode);
    jQuery('div.txtIconRight').css('backgroundColor',colorCode);
}//ChangeControlColor --ends

//...................................................................................................//

// This function can be called to clear the controls.
function ClearControls(arrControlAttributeColl)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
        var currentCol=arrControlAttributeColl[i];
        arrcurrentCol= currentCol.split(",");
        var objCurrentCntrl=  document.getElementById(arrcurrentCol[0]);
        if (objCurrentCntrl)
        {
           switch(arrcurrentCol[7])
           {
                   case "TBox":
                   case "Cal":
                   case "Calc":
                   case "Amount":
                   case "Passwd":
                   case "Phone":
                   {
                        objCurrentCntrl.value  = "";
                        //Clear the parent elements also for all the autofills.
                        if(jQuery(objCurrentCntrl).attr('AutoFill')=='true')
                        {
                            //The objCurrentCtrl would be the clone element in this case.
                            jQuery('#ctl00_cphPageContents_'+objCurrentCntrl.id)
                                            .attr('AFText','')
                                            .attr('AutoFillID','')
                                            .val('');
                        }
                        
                        if(objCurrentCntrl.id.toUpperCase().indexOf("AUTOFILLJOB")!=-1)
                        {
                          //Setting COA attribute for autofilljob textboxs in child grid view
                          objCurrentCntrl.setAttribute("COA",document.getElementById('ctl00_cphPageContents_CGVUC_hdnJobCOADefault').value); 
                        }
                   }
                   break;
                   case "DDL":
                   case "EDDL":
                   {
                        objCurrentCntrl.selectedIndex = 0;
                        objCurrentCntrl.setAttribute("MapPreviousSelItem", objCurrentCntrl.options[0].value); 
                   }
                   break;
                   case "Check":
                   {
                        if (objCurrentCntrl.checked ==true)
                            objCurrentCntrl.checked  = false;
                   }
                   break;
                   case "LBox":
                   {
                        if (objCurrentCntrl.options.length > 0)
                           objCurrentCntrl.selectedIndex = 0;
                   }
                   break;
                   default:
                   break;
           }//Switch
        }//If
    }// For Loops Ends
}//ClearControls ---ends

//...................................................................................................//

// This function can be called to set the default values of the controls.
function SetCntrlDefault(arrControlAttributeColl)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
        var currentCol=arrControlAttributeColl[i];
        arrcurrentCol=currentCol.split(",");
        var objCurrentCntrl=document.getElementById(arrcurrentCol[0]);
        //Don't set defaults to Child Grid Elements.
        if(arrcurrentCol[0].indexOf('CGVUC')!=-1) {            
            continue;
        } 
        if (objCurrentCntrl)
        {
            if(arrcurrentCol[1]!="-1")//DefaultValue
            {
               switch(arrcurrentCol[7])
               {
                       case "TBox":
                       case "Cal":
                       case "Calc":
                       case "Amount":
                       case "Passwd":
                       case "Phone":
                       case "Lbl":
                       {
                            var defVal=unescapeHTML(eval(arrcurrentCol[1]));
                            objCurrentCntrl.value = defVal;
                       }
                       break;
                       case "DDL":
                       case "EDDL":
                       {
                            var dataValueField = "";
                            var strarr = objCurrentCntrl.options[0].value.split('~');
                            //Getting TrxType
                            if (strarr[1] != "")
                            {
                                dataValueField = trim(eval(arrcurrentCol[1])) + "~" + strarr[1];
                            }
                            if (dataValueField != "")
                            {
                                setSelectedIndex(objCurrentCntrl, dataValueField);
                                objCurrentCntrl.setAttribute("MapPreviousSelItem", objCurrentCntrl.options[objCurrentCntrl.selectedIndex].text);
                            }
                       }
                       break;
                       case "Check":
                       {
                            var defVal=eval(arrcurrentCol[1]);
                            if (defVal == "1")
                                objCurrentCntrl.checked = true;
                            else
                                objCurrentCntrl.checked = false;
                       }
                       break;
                       default:
                       break;
               }//Switch
            }
        }//If
    }// For Loops Ends
}//SetDefault---ends


//Set the defaults separately for Child grid.
//Add the onfocus event and enable defaults for all modes i.e Add/Clone/Modify/Find
function SetCGDefaults(arrCtrls)
{
    var arrcurrentCol=new Array();
    for(var cntr=0; cntr<arrCtrls.length;cntr++)
    {
        var currentCol=arrCtrls[cntr];
        arrcurrentCol=currentCol.split(",");
        var el=document.getElementById(arrcurrentCol[0]);
        //Don't set defaults to Child Grid Elements.
        if(arrcurrentCol[0].indexOf('CGVUC')!=-1) {            
            if (el) {
                if(arrcurrentCol[1]!="-1")//DefaultValue
                {
                    var cType=arrcurrentCol[7];
                    var jqEl=jQuery(el);
                    switch(cType)
                    {
                        case "TBox":
                        case "Cal":
                        case "Calc":
                        case "Amount":
                        case "Passwd":
                        case "Phone":
                        case "Lbl":
                        {
                            if(jqEl.val().length==0) {
                                var setVal=unescapeHTML(eval(arrcurrentCol[1]));
                                
                                if(jqEl.attr('AutoFill')=='true' || jqEl.attr('AutoFill')=='1') {
                                    //AutoFill Textbox
                                    //Set the clone as well as its parent.
                                    var clpID='ctl00_cphPageContents_'+el.id;
                                    if(jqEl.attr('AutoFill')=='1') {
                                        clpID=el.id;
                                    }
                                    var split = setVal.split('~');
                                    var AfText='',AfValue;
                                    AfValue=split[0]+'~'+split[1];
                                    if(split.length==3) {
                                        AfText=split[2];
                                    }
                                    else {
                                        //If tilde symbols are present in the text reframe the text excluding TrxId and TrxType
                                        for(var i=2;i<split.length;i++) {
                                            AfText+=split[i];
                                            if(i!=split.length-1) {
                                                AfText+='~';
                                            }
                                        }
                                    }
                                    jqEl.bind('focus',{txt:AfText,val:AfValue,AFParent:clpID},function(e){
                                        var txt=e.data.txt;
                                        var afVal=e.data.val;
                                        jQuery('#'+e.data.AFParent).val(afVal).attr('AutoFillID',afVal).attr('AFText',txt);//Parent
                                        //Set the needed attributes and the remove the focus event.
                                        var elClone=jQuery(this);
                                        elClone.val(txt).attr('AutoFillID',afVal).attr('AFText',txt).unbind('focus');//Clone
                                        setTimeout(function(){ elClone.select() },40);
                                    });
                                }
                                else {
                                    //Normal Textboxes
                                    jqEl.bind('focus',{val:setVal},function(e){
                                        this.value=e.data.val;
                                        jQuery(this).unbind('focus');
                                    });
                                }
                            }             
                            break;
                        }
                        case "DDL":
                        case "EDDL":
                        {
                            var val = "";
                            var strarr = el.options[0].value.split('~');
                            //Getting TrxType
                            if (strarr[1] != "")
                            {
                                val = trim(eval(arrcurrentCol[1])) + "~" + strarr[1];
                            }
                            if (val.length>0 && el.selectedIndex==0)
                            {
                                jqEl.bind('focus',{val:val},function(e){
                                    setSelectedIndex(this, e.data.val);
                                    this.setAttribute("MapPreviousSelItem", this.options[this.selectedIndex].text);
                                    jQuery(this).unbind('focus');
                                });
                            }
                            break;
                        }
                        case 'Check':
                        {
                            var val=eval(arrcurrentCol[1]);
                            if(jqEl[0].checked==false)
                            {
                                jqEl.bind('focus',{val:val},function(e){
                                    this.checked=(e.data.val=='1')?true:false;
                                    jQuery(this).unbind('focus');
                                });
                            }
                            break;
                        }
                        default:{break;}
                    }
                }
            }
        }        
    }
}
//...................................................................................................//

//Sets the MapPreviousSelItem of the Branch DDLs to their current selected text.
function SetSelectedItem(arrControls)
{
    for(var i=0; i<arrControls.length;i++)
    {
        var arrcurrentCol= arrControls[i].split(",");
        var objCurrentCntrl=document.getElementById(arrcurrentCol[0]);
        if (objCurrentCntrl)
        {
           switch(arrcurrentCol[7])
           {
                case "DDL":
                case "EDDL":
                {
                    //Check if the current DDL is a branch parent DDL
                    var attOnChange=objCurrentCntrl.getAttribute("onchange");
                    if(attOnChange==null)  {
                        //Chrome&Safari
                        attOnChange=objCurrentCntrl.onchange;
                    }
                    if(attOnChange&&attOnChange.toString().indexOf('OnBranchDropDownChange')!=-1)  {
                        objCurrentCntrl.setAttribute("MapPreviousSelItem",objCurrentCntrl.options[objCurrentCntrl.selectedIndex].text);
                    }
                    break;
                }
                default:
                    break;
           }//Switch
        }//If
    }// For Loops Ends
}

// This function can be called to hide the TableRow which contains a control whose IsDisplayOnly=1.
//For ChildGridView, should be disabled and change color. If normal controls, hide it.
function HideControls(arrControlAttributeColl, mode)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
        var currentCol=arrControlAttributeColl[i];
        //This array consists of cntrlId, attributes, trId and all
        arrcurrentCol= currentCol.split(",");

        var trObjCurrentCntrl =document.getElementById(arrcurrentCol[8]);
        if (trObjCurrentCntrl)
        {
            if(arrcurrentCol[2]=="1")//IsDisplayOnly
            {
                switch(mode)
                {
                    case "Add":
                    case "Find":
                    {
                        var arrElements = arrcurrentCol[0].split("_"); //Splitting the cntrlId.
                        if(arrElements.length==6)//means gridview
                        {
                            var objCurrentCntrl =document.getElementById(arrcurrentCol[0]);
                            if(objCurrentCntrl)
                            {
                                objCurrentCntrl.disabled=true;
                                objCurrentCntrl.style.backgroundColor = '#d3d3d3';
                            }
                        }
                        else
                            trObjCurrentCntrl.style.display="none";
                    }
                    break;
                    default:
                    break;
                }
            }
        }//If
    }// For Loops Ends
}//HideControls---ends

//...................................................................................................//

var g_AmountFieldJS;
//Disable Enable RegularexpressionValidator and add/remove javascript fucntion FilterAmount
function ShowHideRegularExpressionValidator(arrControlAttributeColl, mode)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length; i++)
    {
        var currentCol=arrControlAttributeColl[i];
        arrcurrentCol= currentCol.split(",");
        
        if(arrcurrentCol[4]!="-1" && arrcurrentCol[4]=="1")//IsNumeric
        {
            //regular expression validation
            var regObjCurrentCntrl=  document.getElementById(arrcurrentCol[9]);
            if (regObjCurrentCntrl)
            {
                regObjCurrentCntrl.display = 'none';
            }

            //Add / Remove FilterAmount Script
            if(arrcurrentCol[7]!="-1" && arrcurrentCol[7]=="Calc")//Control Type
            {
                var objCurrentCntrl=  document.getElementById(arrcurrentCol[0]);
                if (objCurrentCntrl)
                {
                    //Changed by Danny on 24-4-09
                    if (mode=="Find") {
                        //Stop blinking elements if any are blinking from earlier on.
                        if(typeof ResetBlinkObjects=='function')//Because not all pages are referencing ChildGridView.js
                        {
                            ResetBlinkObjects();//Function found in ChildGridView.js
                        }
                        var attOnBlur=objCurrentCntrl.getAttribute("onblur");
                        g_AmountFieldJS=attOnBlur;
                        objCurrentCntrl.removeAttribute('onblur');
                    }
                    else {
                        var attOnBlur=objCurrentCntrl.getAttribute("onblur");
                        if(attOnBlur==null||trim(attOnBlur).length==0)
                        {
                            objCurrentCntrl.setAttribute("onblur", g_AmountFieldJS);
                        }
                    }
                }
            } 
        }//IsNumeric 
    }// For Loops Ends
}//ShowHideRegularExpressionValidator---ends

//...................................................................................................//

//This function can be called to hide the calender control depending on the mode.
function ShowHideCalendar(arrControlAttributeColl, mode)
{
    var arrcurrentCol=new Array();
    for(var i=0; i<arrControlAttributeColl.length;i++)
    {
        var currentCol=arrControlAttributeColl[i];
        arrcurrentCol= currentCol.split(",");
        if(arrcurrentCol[7]!="-1" && arrcurrentCol[7]=="Cal")//Control Type
        {
            if(arrcurrentCol[5]!="-1" && arrcurrentCol[5]=="0")//IsHidden
            {
                 var imgCalender= document.getElementById("ctl00_cphPageContents_img" + arrcurrentCol[10]);//Label   
                 if(imgCalender)
                 {
                     if(mode =="Add" || mode=="Clone" || mode=="Modify")
                     {//check DisplayOnly attr for Modify mode(Let it b happen for other modes also.(issue:33821))
                        if(arrcurrentCol[2]!="-1" && arrcurrentCol[2]!="1")
                            imgCalender.style.display = 'Block';
                        else
                            imgCalender.style.display = 'none';
                     }
                     else if(mode=="Find")
                     {
                        if(arrcurrentCol[6]=="0")//IsSearched
                            imgCalender.style.display = 'none';
                        else
                            imgCalender.style.display = 'Block';
                     }
                     else if(mode=="Delete" || mode=="Select")
                     {
                        imgCalender.style.display = 'none';
                     }
                 }
            }
        }
    }// For Loops Ends
}//ShowHideCalendar---ends

//...................................................................................................//

//To select a particular dropdown value.(For set default)
function setSelectedIndex(objddl, value)
{
    for ( var i = 0; i < objddl.options.length; i++ ) 
    {
        if ( objddl.options[i].value == value ) 
        {
            objddl.options[i].selected = true;
            return;
        }
    }
}

//...................................................................................................//

//This function can be called to reset all the buttons depending on the mode.
function ShowHideFormButtons(mode, arrControlAttributeColl, g_cdnImagesPath)
{
    var imgbtnContinueAdd=document.getElementById('ctl00_cphPageContents_imgbtnContinueAdd');
    var imgAddClone=document.getElementById('ctl00_cphPageContents_imgbtnAddClone');
    var imgBtnSubmit=document.getElementById('ctl00_cphPageContents_imgbtnSubmit');
    var imgBtnCancel=document.getElementById('ctl00_cphPageContents_imgbtnCancel');
    var hdnSubmitstatus = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnSubmitstatus');
    var lblMessage = document.getElementById('ctl00_cphPageContents_lblmsg');

    if(hdnSubmitstatus.value!="")//In Submit
        lblMessage.style.display = 'Block';
    else
        lblMessage.style.display = 'none';

     //Hide the trProcessLinks in Add/Add-Clone/Modify/Find/Delete.
     HideProcessLinks();
     
     //Exclusivly for SOX Approval Tr removal
     var trSOX=document.getElementById('ctl00_cphPageContents_trSoxApprovedStatus');
     if (trSOX)
        trSOX.style.display = 'none';
        //trSOX.style.visibility='hidden';
     
     //Show CalendarControl
     ShowHideCalendar(arrControlAttributeColl, mode);

     imgBtnSubmit.style.display = 'Block';
     imgBtnCancel.style.display = 'Block';

    switch (mode)
    {
        case "Add":
        case "Clone":
        {
             //Form level buttons
             imgbtnContinueAdd.style.display = 'Block';
             imgAddClone.style.display = 'Block';

             var hdnNeedToConfirmExit = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnNeedToConfirmExit');
             hdnNeedToConfirmExit.value = "True";
             
             ResetJobTypeImage(g_cdnImagesPath);
        }
        break;
        case "Modify":
        {
            //Form level buttons
            imgbtnContinueAdd.style.display = 'none';
            imgAddClone.style.display = 'none';

            var hdnNeedToConfirmExit = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnNeedToConfirmExit');
            hdnNeedToConfirmExit.value = "True";
        }
        break;
        case "Delete":
        {
            //Form level buttons
            imgbtnContinueAdd.style.display = 'none';
            imgAddClone.style.display = 'none';
        }
        break;
        case "Find":
        {
            //Form level buttons
            imgbtnContinueAdd.style.display = 'none';
            imgAddClone.style.display = 'none';

            ResetJobTypeImage(g_cdnImagesPath);
        }
        break;
        case "Select":
        {
             if (trSOX)
                trSOX.style.display = '';
             
             //Form level buttons
             imgbtnContinueAdd.style.display = 'none';
             imgAddClone.style.display = 'none';
             
             lblMessage.style.display = 'Block';
             
             //Find function in GVUC.js
             if(typeof(ValidateBPGIDRow)=='function')
                ValidateBPGIDRow();
             //Show process links in View mode(like Submit, RdbClk, Modifyload cases)
             ShowProcessLinks();
             
             imgBtnSubmit.style.display = 'none';
             imgBtnCancel.style.display = 'none';
        }
        break;
        default:
        break;
    }//Switch case
}//ShowHideFormButtons---ends

//...................................................................................................//

function ShowProcessLinks()
{
    var trProcessLinks=document.getElementById('ctl00_cphPageContents_trProcessLinks');
    if (trProcessLinks)
    {
        //trDynamicProcessLinks, the dynamic links generated in trProcessLinks->pnlBPCContainer->Table.
        var trDynamicProcessLinks=document.getElementById('ctl00_cphPageContents_trDynamicProcessLinks');
        if(trDynamicProcessLinks)
        {       
            if (trDynamicProcessLinks.cells.length > 0)
            {
                var noenabledCntrls = CheckProcLnksVisibility(trDynamicProcessLinks);
                if(noenabledCntrls>0)
                {        
                    if(document.getElementById('ctl00_cphPageContents_pnlGVContent')!=null)
                    trProcessLinks.style.display = '';//Blocking the trProcessLinks
                }
                else
                    trProcessLinks.style.display = 'none';
            }
            else
                trProcessLinks.style.display = 'none';
        }
        else
            trProcessLinks.style.display = 'none';
    }//trProcessLinks
}//ShowProcessLinks---ends

//...................................................................................................//
//Added by Shanti to invisible trProcessLinks if no enable controls in that tr.
function CheckProcLnksVisibility(trDynamicProcessLinks)
{
     var i=0;
     for(var j=0; j<trDynamicProcessLinks.cells.length; j++)
     {
        var objCell=trDynamicProcessLinks.cells[j].firstChild.firstChild;
        if(objCell== null) { // The cell consists of the separator.
            continue;
        }
        if(trDynamicProcessLinks.cells[j].style.display=="" || trDynamicProcessLinks.cells[j].style.display=="Block")
        {
            i++;
        }
     }
     return i;
}

//...................................................................................................//

function HideProcessLinks()
{
    var trProcessLinks=document.getElementById('ctl00_cphPageContents_trProcessLinks');
    if(trProcessLinks)
    {
        trProcessLinks.style.display = 'none';
        //trProcessLinks.style.visibility = 'hidden';
    }
}

//...................................................................................................//

//This function can be called to disable/enable all the BtnsUC buttons and change the buttons urls to disable/enable urls.
function ResetActionButtons(g_cdnImagesPath, mode)
{ 
    var hdnRwXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnRwToBeModified').value;
    //getting the control objects
    var imgbtnAdd = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnAdd');
    var imgbtnClone = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnClone');
    var imgbtnModify = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnModify');
    var imgbtnDelete = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnDelete');
    var imgbtnFind = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnFind');
    var imgbtnNote = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnNote');
    var imgbtnSecure = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnSecure');
    var imgbtnAttachment = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnAttachment');
    var imgbtnPrint = document.getElementById('ctl00_cphPageContents_BtnsUC_imgbtnPrint'); 
 
   //To set the button images depending on enability of images.
    if(imgbtnAdd)
    {
        if(mode=="PageLoad")
        {
            imgbtnAdd.disabled=false;
            imgbtnAdd.src =  g_cdnImagesPath + "Images/add-icon.png";
            imgbtnAdd.style.cursor="pointer";//show hand symbol for enabled controls.(Issue no-33904)
        }
        else if(mode!="Select")
        {
            imgbtnAdd.disabled=true;
            imgbtnAdd.src =  g_cdnImagesPath + "Images/add-icon-disable.png";
            imgbtnAdd.style.cursor="default";//dont show hand symbol for disabled controls.(Issue no-33904)
        }
        else
        {
            imgbtnAdd.disabled=false;
            imgbtnAdd.src =  g_cdnImagesPath + "Images/add-icon.png";
            imgbtnAdd.style.cursor="pointer";
        }        
    }
    if(imgbtnModify)
    {
        if(mode!="Select")
        {
            imgbtnModify.disabled=true;
            imgbtnModify.src =  g_cdnImagesPath + "Images/modify-icon-disable.png";
            imgbtnModify.style.cursor="default";
        }
        else
        {
            if(hdnRwXML!="")//If cancel ll send 'select' where all btns enabled except in norows case.so checking 
            {
                imgbtnModify.disabled=false;
                imgbtnModify.src = g_cdnImagesPath + "Images/modify-icon.png";
                imgbtnModify.style.cursor="pointer";
            }
        }
    }
    if (imgbtnClone)
    {
        if(mode!="Select")
        {
            imgbtnClone.disabled=true;
            imgbtnClone.src =  g_cdnImagesPath + "Images/lite--add-clone.png";
            imgbtnClone.style.cursor="default";
        }
        else
        {
            if(hdnRwXML!="")
            {
                imgbtnClone.disabled=false;
                imgbtnClone.src =  g_cdnImagesPath + "Images/add-clone.png";
                imgbtnClone.style.cursor="pointer";
            }
        }
    }
    if(imgbtnDelete)
    {
        if(mode!="Select")
        {
            imgbtnDelete.disabled=true;
            imgbtnDelete.src =  g_cdnImagesPath + "Images/delete-icon-disable.png";
            imgbtnDelete.style.cursor="default";
        }
        else
        {
            if(hdnRwXML!="")
            {
                imgbtnDelete.disabled=false;
                imgbtnDelete.src =  g_cdnImagesPath + "Images/delete_icon.png";
                imgbtnDelete.style.cursor="pointer";
            }
        }
    }
    if(imgbtnFind)
    {
        if(mode=="PageLoad")
        {
            imgbtnFind.disabled=false;
            imgbtnFind.src = g_cdnImagesPath + "Images/find-icon.png";
            imgbtnFind.style.cursor="pointer";
        }
        else if(mode!="Select")
        {
            imgbtnFind.disabled=true;
            imgbtnFind.src = g_cdnImagesPath + "Images/find-icon-disable.png";
            imgbtnFind.style.cursor="default";
        }
        else
        {
            imgbtnFind.disabled=false;
            imgbtnFind.src = g_cdnImagesPath + "Images/find-icon.png";
            imgbtnFind.style.cursor="pointer";
        }
    }
    if(imgbtnNote)
    {
        if(mode!="Select")
        {
            imgbtnNote.disabled=true;
            imgbtnNote.src = g_cdnImagesPath + "Images/footnote-icon-disable.png";
            imgbtnNote.style.cursor="default";
        }
        else
        {//Canceling in SelectInvoicepage--mode select. No way to call 'NoteAttachSecurePicChange' so doing explicitly.
            var currentPage = window.location + "";//Cast the object to string.
            if(currentPage.toUpperCase().indexOf('SELECTINVOICE.ASPX') != -1)
            {
                imgbtnNote.disabled=false;
                imgbtnNote.src = g_cdnImagesPath + "Images/footnote-icon.png";
                imgbtnNote.style.cursor="pointer";
            }
            //else//It is been handled in serverside, changing the url using NoteAttachSecurePicChange method.
        }
    }
    if(imgbtnSecure)
    {
        if(mode!="Select")
        {
            imgbtnSecure.disabled=true;
            imgbtnSecure.src = g_cdnImagesPath + "Images/security-icon-disable.png";
            imgbtnSecure.style.cursor="default";
        }
        else
        {
            var currentPage = window.location + "";//Cast the object to string.
            if(currentPage.toUpperCase().indexOf('SELECTINVOICE.ASPX') != -1)
            {
                imgbtnSecure.disabled=false;
                imgbtnSecure.src = g_cdnImagesPath + "Images/security-icon.png";
                imgbtnSecure.style.cursor="pointer";
            }
        }
    }
    if(imgbtnAttachment)
    {
        if(mode!="Select")
        {
            imgbtnAttachment.disabled=true;
            imgbtnAttachment.src = g_cdnImagesPath + "Images/attachment-icon-disable.png";
            imgbtnAttachment.style.cursor="default";
        }
        else
        {
            var currentPage = window.location + "";//Cast the object to string.
            if(currentPage.toUpperCase().indexOf('SELECTINVOICE.ASPX') != -1)
            {
                imgbtnAttachment.disabled=false;
                imgbtnAttachment.src = g_cdnImagesPath + "Images/attachment-icon.png";
                imgbtnAttachment.style.cursor="pointer";
            }
        }
    }
    if(imgbtnPrint)
    {
        if(mode!="Select")
        {
            imgbtnPrint.disabled = true;
            imgbtnPrint.src = g_cdnImagesPath + "Images/print-icon-disable.png";
            imgbtnPrint.style.cursor="default";
        }
        else
        {
            if(hdnRwXML!="")
            {
                imgbtnPrint.disabled = false;
                imgbtnPrint.src = g_cdnImagesPath + "Images/print-icon.png";
                imgbtnPrint.style.cursor="pointer";
            }
        }
    }
}//ResetActionButtons---ends

//...................................................................................................//

//This function can be called to change the submit url depending on the mode.
function SetSubmitURL(g_cdnImagesPath)
{
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction').value;
    if(hdnCurrAction=="Clone")
        hdnCurrAction="Add";

    //changing the image url of common 'Submit' button to current action.
    var imgBtnSubmit=document.getElementById('ctl00_cphPageContents_imgbtnSubmit');
    imgBtnSubmit.src =  g_cdnImagesPath + "Images/"+hdnCurrAction+"-but.png";
}

//...................................................................................................//


//called in Submit onclientclick to validate controls as well as to test IsOkToMOdify/IsOkToDelete.
function ValidateControls()
{
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction').value;    
    if(hdnCurrAction=="Add" || hdnCurrAction=="Clone")
    {
        return Page_ClientValidate("LAJITEntryForm");
    }
    else if(hdnCurrAction=="Modify")
    {
      if(typeof(Page_ClientValidate)!='function' || Page_ClientValidate("LAJITEntryForm")) //typeof(Page_ClientValidate)!='function' || added by Danny
         return TestAttribute("OkToUpdate");
      else
         return false;
    }
    else//for all the other cases like delete/find
    {
        if( Page_ClientValidate("none"))
        { 
            if(hdnCurrAction=="Delete")
             return TestAttribute("OkToDelete");
            else
             return true;
        }
        else
            return false;
    }
}

//...................................................................................................//

//To test IsOkToMOdify/IsOkToDelete of particular row.
function TestAttribute(strAttribute)
{
    var hdnRwXML = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnRwToBeModified').value;
    var xDocRowXML = loadXMLString(hdnRwXML);
    if(xDocRowXML.firstChild.getAttribute(strAttribute)!=null && xDocRowXML.firstChild.getAttribute(strAttribute)=="0")
    {
       (strAttribute == "OkToDelete") ? alert("This Record can not be deleted.") : alert("This Record can not be modified.");
        return false;
    }
    else
        return true;
}

//...................................................................................................//

//This function can be called to open the pnlEntryForm.
function OpenEntryForm()
{
    var pnlEntryForm=document.getElementById('ctl00_cphPageContents_pnlEntryForm');
    if(pnlEntryForm)
        pnlEntryForm.style.display = 'Block';
}

//...................................................................................................//

//This function can be called to close the pnlEntryForm.
function CloseEntryForm()
{
    var pnlEntryForm=document.getElementById('ctl00_cphPageContents_pnlEntryForm');
    if(pnlEntryForm)
        pnlEntryForm.style.display = 'none';
}

//...................................................................................................//

//This function can be called to show the pnlGVContent.
function BlockGrid()
{
    var pnlGVContent=document.getElementById('ctl00_cphPageContents_pnlGVContent');
    if(pnlGVContent)
        pnlGVContent.style.display = 'Block';
}

//This function can be called to hide the pnlGVContent.
function HideGrid()
{
    var pnlGVContent=document.getElementById('ctl00_cphPageContents_pnlGVContent');
    if(pnlGVContent)
        pnlGVContent.style.display = 'none';
}

//...................................................................................................//

//This function can be called to expand the grid.
function ExpandGrid()
{
//     var cpeCntrlpnlTitleobj = $find("BIDMainGrid");
//     if(cpeCntrlpnlTitleobj)
//     {
//        if(cpeCntrlpnlTitleobj._checkCollapseHide())//If hidden, expand it.
//            cpeCntrlpnlTitleobj.expandPanel();
//     }

//Expands
 jQuery('#ctl00_cphPageContents_pnlGVContent').slideDown(200);
}

//...................................................................................................//

//This function can be called to collapse the grid.
function CollapseGrid()
{
//     var cpeCntrlpnlTitleobj = $find("BIDMainGrid");
//     if(cpeCntrlpnlTitleobj)
//     {
//        if(!cpeCntrlpnlTitleobj._checkCollapseHide())//If not hidden, collapse it.
//            cpeCntrlpnlTitleobj.collapsePanel();
//     }

        //var themeSession = document.getElementById('ctl00_hdnThemeName').value;
        //var imagesCDNPath = document.getElementById('ctl00_hdnImagesCDNPath').value;
        
//      jQuery('#ctl00_cphPageContents_pnlGVContent').slideToggle('slow');
        jQuery('#ctl00_cphPageContents_pnlGVContent').slideUp(200);//collpase
        var imgIcon=document.getElementById('imgCPGV1Expand').getAttribute('src');
        var imgIconSrc=imgIcon.split('/');
        if(imgIconSrc[4]=="plus-icon.png")
        {
            jQuery("#imgCPGV1Expand").attr("src",g_cdnImagesPath+"Images/minus-icon.png");
        }
        else 
        {
            jQuery("#imgCPGV1Expand").attr("src",g_cdnImagesPath+"Images/plus-icon.png");
        }
}

//...................................................................................................//

function ResetJobTypeImage(g_cdnImagesPath)
{
    var imgTypeOfJob=document.getElementById('ctl00_cphPageContents_imgTypeOfJob');
    //Set a dummy image source.
    if (imgTypeOfJob)
    {
        imgTypeOfJob.src="~/App_Themes/" + g_cdnImagesPath + "Images/spacer.gif";
        imgTypeOfJob.style.display = 'none';
    }
}

//...................................................................................................//

//This funtion will be called in AddLoad case where is no BPC for Add.
function ResetCurrAction()
{
    var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');    
    hdnCurrAction.value = "";
}

//...................................................................................................//

function ResetChildGridAmount()
{
    try
    {
        var objGridView  = document.getElementById('ctl00_cphPageContents_CGVUC_tblAmounts');
        if(objGridView)
        {
            for( var i = 0; i < objGridView.rows.length; i++)
            {
               tdamt = objGridView.rows[i].cells;
                if(tdamt)
                {
                   for( var j = 0; j < tdamt.length; j++)
                   {
                    //GetFirstChild function exist in GridViewUserControl.js
                     var objspan=GetFirstChild(objGridView.rows[i].cells[j]);
                     objspan.innerHTML="0.00";
                   }
                }
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
}//ResetChildGridAmount---end

//...................................................................................................//

//Set Focus in Fist Element in Entry Panel
function SetFocusFirstItem()
{ 
  //This GetFrmElements exist in common.js file

  
  //GetFrmElements().eq(0).focus();

   //Get all the textboxesid,buttonids,imagesids,dropdownlist Ids
  //var fields = jQuery("input:visible:enabled,select:visible:enabled");
  var fields =GetFrmElements();
   if(fields.length > 0)
   {
      for(i=0;i<fields.length;i++)
      {  
         if(fields[i].type=="select-one")
         {
            //Dropdownlist 
            if( ((fields[i].id.indexOf('ctl00_cphPageContents_') != -1) && (fields[i].id.indexOf('ctl00_cphPageContents_GVUC_') == -1)))
            {
                jQuery("#"+fields[i].id).focus();
                break;
            }
         } 
         if(fields[i].type=="text")
         {
             //Textbox 
              jQuery("#"+fields[i].id).focus();
              break;
         }
      }// for loop end
    } // if end

   //OLD METHOD
   
//   var TBox=document.getElementById('ctl00_cphPageContents_pnlEntryForm').getElementsByTagName('input')[0];
//     if(TBox)
//     {
//         if(TBox.id =="ctl00_cphPageContents_imgbtnIsApproved")
//         {
//            var InnerTBox=document.getElementById('ctl00_cphPageContents_pnlEntryForm').getElementsByTagName('input')[1];
//            var InnerAutofill=InnerTBox.getAttribute('AutoFillID');
//            if(InnerAutofill)
//            {
//               jQuery("input:visible:enabled:first").focus();
//            }
//            else
//            {
//               InnerTBox.focus();
//            }
//            
//           // document.getElementById('ctl00_cphPageContents_pnlEntryForm').getElementsByTagName('input')[1].focus();         
//         }
//         else
//         {  
//           var Autofill=TBox.getAttribute('AutoFillID');
//           if(InnerAutofill)
//           {
//             jQuery("input:visible:enabled:first").focus();
//           }
//           else
//           {
//            if(TBox.Visibility=="Block")
//            TBox.focus();
//            }
//         }
//      }
  
   //jQuery("input:visible:enabled:first").focus();

     
}// End of Set Focus in Fist Element in Entry Panel

//...................................................................................................//

function UpdateRadioNotesData(index)
{
    var radObj=document.getElementsByName("rbtngvCell")[index];
    document.getElementById("ctl00_cphPageContents_BtnsUC_hdnSectedRow").value=radObj.value;
    var hdnObj=document.getElementsByName("hdnNotes")[index];
    document.getElementById("ctl00_cphPageContents_BtnsUC_txtNotes").value=hdnObj.value;
    //alert(hdnObj.value);
} 

//...................................................................................................//

//FootNote and Attachment Scripts Usage
function HideNotesModel()
{
    HideIframeProgress();
    //$find('mpeNotesBehaviourID').hide();
    setTimeout("DisplayPopUp(false,'ctl00_cphPageContents_BtnsUC_pnlNote')",50);
}

//...................................................................................................//

function Openframe(pType,pagename)
{
    var pHeight;
    var pWidth;
    switch (pType)
    {
     case "Note":
                 var control=document.getElementById("ctl00_cphPageContents_BtnsUC_imgbtnNote");
                 var QueryStringValues="?BPGID="+control.getAttribute('BPGID')+"&TrxID="+ control.getAttribute('TrxID')+"&TrxType="+ control.getAttribute('TrxType');
                 pagename=pagename+QueryStringValues;  
                 pHeight='380px';   
                 pWidth='600px';
                 break;
     case "Attachment": 
                 var control=document.getElementById("ctl00_cphPageContents_BtnsUC_imgbtnAttachment");
                 var QueryStringValues="?BPGID="+control.getAttribute('BPGID')+"&TrxID="+ control.getAttribute('TrxID')+"&TrxType="+ control.getAttribute('TrxType');
                 pagename=pagename+QueryStringValues; 
                 pHeight='380px';   
                 pWidth='600px';
                 break;
      case "Secure":
                 var control=document.getElementById("ctl00_cphPageContents_BtnsUC_imgbtnSecure");
                 var QueryStringValues="?BPGID="+control.getAttribute('BPGID')+"&TrxID="+ control.getAttribute('TrxID')+"&TrxType="+ control.getAttribute('TrxType');
                 pagename=pagename+QueryStringValues;
                 pHeight='239px';   
                 pWidth='545px';  
                 break;
    }      
    
   
//    //Setting Pagename
//    var iFrame=$get('iframePage');
//    iFrame.height=pHeight;
//    iFrame.width=pWidth;
//    DisplayPopUp(true);//,'ctl00_cphPageContents_BtnsUC_pnlNote');
//    iFrame.src = "../PopUps/"+pagename;
    ShowIFrame("../PopUps/"+pagename,{width:pWidth,height:pHeight,title:pType,isModal:true,allowMaximise:false,resize:false});
//    ChangeCloseLinkText(pType,this);
    return false;
} 

//...................................................................................................//
   
function Clickheretoprint()
{ 
    var m_innerHtml=document.getElementById("ctl00_cphPageContents_BtnsUC_hdnPrints").value;
    var disp_setting="toolbar=yes,location=no,directories=yes,menubar=yes,"; 
    disp_setting+="scrollbars=yes,width=500, height=200"; 
    var docprint=window.open("","",disp_setting); 
    docprint.document.open(); 
    docprint.document.write('<html><body onLoad="self.print()">');
    docprint.document.write('<table>'+m_innerHtml+'</table>');
    docprint.document.write('</body></html>'); 
    docprint.document.close(); 
 }

//...................................................................................................//

function ShowPDF(redirectPage)
{
    var curobject=document.getElementById('ctl00_cphPageContents_BtnsUC_hdnPrintInfo').value; 
    var cur=curobject.split('~');
    var curobjTrxID=cur[0];
    var curobjTrxType=cur[1]; 
    redirectPage='../PopUps/'+redirectPage+'?PopUp=BTN&TrxID='+curobjTrxID+'&TrxType='+curobjTrxType; 
    window.open(redirectPage,'_blank');
    return false;
}

//...................................................................................................//
//set focus from parent last element to child grid first input element
function SetTabFocusChildGrid()
{
    var objGrid = document.getElementById('ctl00_cphPageContents_CGVUC_grdVwBranch');
    //Check child grid exist or not
    if (objGrid)
    {
        //find out the last parent element
        var parentSelector='tr[id^="ctl00_cphPageContents_tr"] :input:last';
        var elToBlur=jQuery(parentSelector);
        //add on TAB event to set focus to child grid element
        elToBlur.bind('keydown', function(e) { 
            var keyCode = e.keyCode || e.which; 
            if (keyCode == 9) {
                if(e.preventDefault) {
                    e.preventDefault(); 
                }
                // Set default rowindex to zero for first row
                var rowIndex=0;
                var gridId='ctl00_cphPageContents_CGVUC_grdVwBranch';
                //Select first row from child grid view
                var rowSelector="#"+gridId+" tr[id*='GVRow']:eq("+rowIndex+")";
                //Select input text box. here 0 th element is checkbox   
                var inputSelector=":input:visible:eq(1)";
                var target=jQuery(rowSelector).find(inputSelector);
                if(target.size()>0)
                {
                    if(target.closest('tr').css('display')==='none') {
                        //Call recursively for deleted rows.Function found in ChildGridView.js file
                        SetFocusNextRow(rowIndex++,gridId);
                    }
                    else {   
                        //Set focus on child grid first input element
                        target[0].focus();
                    }
                }
            } 
        });
     }  
}

//...................................................................................................//


    
 