//This file contains the frequently called JS functions.
var g_IsDebugging = false;
var g_IFrameWidth = 932; //Values synonomous to those of the IFrames in both the Master Pages.
var g_IFrameHeight = 525;
var g_HelpAuth; //Stores the details of the Help Authoring passed from the server-side in the format navPage~IsPopUp
var g_IFramePage = ""; //Stores help iframe page name
var g_IsDashBoard;
var g_IsPostBack = false;
var g_scrollVertical; // Stores windows scroll vertical width
var g_IFDec; //Specifies the Iframe dimensions reduction factor.
var g_cdnImagesPath; //This values assigned topleft master page
var g_cdnScriptsPath; //This values assigned topleft master page
var g_virtualPath; // This value assigned topleft master page

if (typeof (jQuery) == 'undefined') {
    setTimeout(function () { jQuery(document).ready(DocLoad) }, 1000);
}
else {
    jQuery(document).ready(DocLoad);
}

//Function will be called when the DOM is ready for manipulation.
function DocLoad() {
    //    jQuery.fx.off=true;
    //Init the constants
    g_IFDec = new Object();
    g_IFDec.Width = 15;
    g_IFDec.Height = 26;
    try {//Base window
        var width, height;
        var $win = jQuery(top);
        width = $win.width();
        height = $win.height();
        //Set the form width leaving 12px for padding on the either side.
        $get('tblMaster').width = eval(width - 24) + 'px';
        top.g_IFrameWidth = width - (width * 0.075); //7.5%
        top.g_IFrameHeight = height - (height * 0.14); //14%
    }
    catch (e) {//Iframes
        g_IFrameWidth = top.g_IFrameWidth;
        g_IFrameHeight = top.g_IFrameHeight;
    }
    AdjustTitles();



}

//Page Load event for the current page
//This event will be called after the first page load and for every async postback.
function pageLoad(sender, args) {

    //Page Key Press Handler.
    $addHandler(document, "keydown", OnKeyPress);
    //To override the space above CPGV1 occuring in Dashboard.
    var currentPage = window.location + ""; //Cast the object to string.
    if (currentPage.toUpperCase().indexOf('DASHBOARD.ASPX') != -1) {
        $get('ctl00_cphPageContents_pnlExpViewContent').style.height = '0px';
        g_IsDashBoard = true;
    }
    else {
        g_IsDashBoard = false;
    }
    SetParentGridLinks();
    AdjustTitles();
    if (typeof (SetTabFocusChildGrid) == 'function') SetTabFocusChildGrid();
    //Only after the page has completely loaded, allow row-hovering effects.
    g_IsPostBack = true; //pageload will be called after all the other js functiona have finished executing.So it is ok to put this here.
    JQueryCaller();

}

//...................................................................................................//
//Checks for the Grid Headers to adjusted properly if in case the grid has too many columns
var g_IntialTitleWidth;
function AdjustTitles() {
    var pnlHeaderTitle = $get('ctl00_cphPageContents_pnlCPGV1Title');
    var pnlEntryForm = $get('ctl00_cphPageContents_pnlEntryForm');
    //    var exec=true;
    //    if(g_PostBackElement && g_PostBackElement.id.indexOf('imgBtnTTnNavigate') != -1) {
    //        exec=false;
    //    }
    if (pnlHeaderTitle && pnlEntryForm) {
        //        var pnlHdrAuto=$get('ctl00_cphPageContents_htcCPGV1Auto');
        //        if(typeof(g_IntialTitleWidth)=="undefined")
        //        {
        //            var elementBounds = Sys.UI.DomElement.getBounds(pnlHeaderTitle);
        //            g_IntialTitleWidth = elementBounds.width;
        //        }
        //        if(g_IntialTitleWidth>768)//Do the adjustment only for the grids which have many columns
        //        {
        //            var expectedWidth=g_IntialTitleWidth-28-$get('ctl00_cphPageContents_htcCPGV1').width;
        //            pnlHdrAuto.width=expectedWidth+"px";
        //            pnlEntryForm.style.width=g_IntialTitleWidth+"px";
        //            pnlHeaderTitle.style.width=g_IntialTitleWidth+"px";
        //        }


        var cgvuc = $get('ctl00_cphPageContents_CGVUC_pnlGVBranch');
        var currentPage = window.location + ""; //Cast the object to string.
        var isCommercial = (currentPage.toUpperCase().indexOf('COMMERCIAL.ASPX') != -1) ? true : false;
        if (cgvuc && !isCommercial) {
            var setWidth = Sys.UI.DomElement.getBounds(pnlHeaderTitle).width - 2;
            cgvuc.style.width = setWidth + "px";
        }
    }
}

//...................................................................................................//

function JQueryCaller() {
    try {
        //JQuery Stuff.
        InitPopUps();
        if (typeof InitToolTips === 'function') {
            InitToolTips(); //Function in GridViewUserControl.js
        }
    }
    catch (error) {
        if (g_IsDebugging) {
            alert(error.message);
        }
    }
    if (typeof JQueryEvents === 'function') {
        JQueryEvents(); //From the page-level Sys.AddLoad
    }
}

//...................................................................................................//

function InitPopUps(init) {
    jQuery(document).ready(
            function () {
                var targetDiv;
                if ($get('ctl00_pnlPagePopUp') != null) {
                    targetDiv = '#ctl00_pnlPagePopUp';
                }
                else {
                    targetDiv = '#pnlPagePopUp';
                }

                var jqObj = jQuery(targetDiv);
                if (jqObj.length != 0) {
                    var jqDialog = jqObj.dialog({ autoOpen: false,
                        bgiframe: false,
                        width: 'auto',
                        height: 'auto',
                        modal: true,
                        //			                         show:'clip',
                        //			                         hide:'clip',
                        //show:1000,
                        draggable: false,
                        resizable: false,
                        closeOnEscape: false
                    });

                    jqObj.removeClass("ui-dialog-content");
                    jqDialog.parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                }

                //Exploded View Initialisation.
                jqObj = jQuery('#ctl00_pnlExplodedView');
                if (jqObj.length != 0) {
                    jqObj.dialog({ autoOpen: false,
                        bgiframe: false,
                        width: 'auto',
                        position: 'center',
                        minHeight: 10,
                        modal: true,
                        draggable: false,
                        resizable: false
                    });
                    jqObj.removeClass("ui-dialog-content ui-widget-content");
                    jqObj.dialog().parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                }

                //Print PopUp in ProcessEngine
                jqObj = jQuery('#ctl00_cphPageContents_pnlPrint');
                if (jqObj.length != 0) {
                    jqObj.dialog({ autoOpen: false,
                        bgiframe: false,
                        width: 'auto',
                        position: 'center',
                        minHeight: 10,
                        modal: true,
                        draggable: false,
                        resizable: false
                    });
                    jqObj.removeClass("ui-dialog-content ui-widget-content");
                    jqObj.dialog().parents(".ui-dialog").find(".ui-dialog-titlebar").remove();
                }

            }
         );
}

//...................................................................................................//

function DisplayPopUp(visible, divId) {
    //f_scrollTop method defination exist at Utility.js file 
    var g_scrollVertical = f_scrollTop();

    var targetDiv;
    if (typeof divId != 'undefined') {
        targetDiv = '#' + divId;
    }
    else {
        if ($get('ctl00_pnlPagePopUp') != null) {
            targetDiv = '#ctl00_pnlPagePopUp'; //Pages with Master Pages
        }
        else {
            targetDiv = '#pnlPagePopUp'; //Pages without :)
        }
    }
    if (visible) {
        jQuery(targetDiv).dialog('open');
    }
    else {
        jQuery(targetDiv).dialog('close');
    }
    //iframe load add event
    var iFrame = $get('iframePage');
    if (iFrame) {
        iFrame.onload = function () { window.scrollTo(0, g_scrollVertical); iFrame.focus(); };
    }
}

//...................................................................................................//

function CheckImages() {
    var arrImages = document.images;
    for (var i = 0; i < arrImages.length; i++) {
        var imgSrc = arrImages[i].src;
        if (imgSrc) {
            imgSrc = trim(imgSrc);
            if (imgSrc.indexOf('gif') == -1 && imgSrc.indexOf('jpg') == -1 && imgSrc.indexOf('jpeg') == -1
                && imgSrc.indexOf('png') == -1 && imgSrc.indexOf('WebResource') == -1) {
                alert('Image not found!!\n' + (arrImages[i].title || arrImages[i].alt));
            }
        }
        else {
            alert('Image src attribute missing!!');
        }
    }
    //Check the script references
    var arrScripts = document.getElementsByTagName('script');
    for (var i = 0; i < arrScripts.length; i++) {
        var scriptRef = arrScripts[i];
        if (trim(scriptRef.src) == '' && arrScripts[i].firstChild && arrScripts[i].firstChild.nodeValue.toString().length == 0) {
            alert('Script not found!!\n' + arrScripts[i].firstChild.nodeValue);
        }
    }
}

//....................................................................................................//
//Called when the form-level process links are clicked upon in the page.
function OnFormBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp, confirmExec) {
    var COXML = "";
    var hdnGVBPEINFO = $get(bpeInfoClientID);
    if (hdnGVBPEINFO && trim(hdnGVBPEINFO.value).length > 0) {
        COXML = hdnGVBPEINFO.value;
    }
    else//Taking BtnsUC hdnBPInfo which is set in Modify Load event
    {
        hdnGVBPEINFO = $get('ctl00_cphPageContents_BtnsUC_hdnGVBPEINFO');
        if (hdnGVBPEINFO) {
            COXML = hdnGVBPEINFO.value;
        }
        else {
            if (g_IsDebugging) {
                alert("No RowXML was found in Hidden variables either in GVUC or BtnsUC!!This alert was raised by Common.js>OnFormBPCLinkClick()");
                return;
            }
        }
    }
    CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML, confirmExec);
}

//...................................................................................................//

var g_CurrentPageName = null;
//Validates and runs the process for a given Business Process Link.
function CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML, optionalParam) {
    var rptType = '';
    if (typeof (g_PDFOptions) != "undefined") {
        redirectPage = redirectPage + "?PN=" + g_PDFOptions.PageNumber + "&WM=" + g_PDFOptions.WaterMark + "&PL=" + g_PDFOptions.PageLayout + "&PS=" + g_PDFOptions.PageSize + "&FN=" + g_PDFOptions.FontName + "&PD=" + g_PDFOptions.Password + "&AT=" + g_PDFOptions.Annotations;
    }
    if (typeof (optionalParam) != "undefined") {
        if (isNaN(optionalParam)) {
            optionalParam = escapeHTML(optionalParam.replace(/~::~/g, " ")); //Restore the spaces.
            if (trim(optionalParam).length > 0) {
                if (!confirm(optionalParam)) return;
            }
        }
        else {
            var reportType = parseInt(optionalParam); //1.PDF 2.Excel 3.Word 4.HTML 
            switch (reportType) {
                case 1:
                    rptType = 'PDF';
                    break;
                case 2:
                    rptType = 'Excel';
                    break;
                case 3:
                    rptType = 'Word';
                    break;
            }
        }
    }
    var noPageProcess = (processName.indexOf("NoPageProcess") != -1) ? true : false;
    var procRefresh = (processName.indexOf("Refresh") != -1) ? true : false;
    var lnkBtnCloseIFrame = $get('ctl00_lnkBtnCloseIFrame');
    var pbHref = "javascript:__doPostBack('ctl00$lnkBtnCloseIFrame','')";
    if (lnkBtnCloseIFrame == null) {
        lnkBtnCloseIFrame = $get('ctl00_cphPageContents_lnkBtnCloseIFrame');
        pbHref = "javascript:__doPostBack('ctl00$lnkBtnCloseIFrame','')";
    }
    if (procRefresh) {
        //If Refresh Process only then remove the onclick and set the href attribute so that it will postback to the server
        if (lnkBtnCloseIFrame) {
            lnkBtnCloseIFrame.setAttribute("onclick", "");
            lnkBtnCloseIFrame.setAttribute("href", pbHref);
        }
    }
    else {
        if (lnkBtnCloseIFrame) {
            lnkBtnCloseIFrame.setAttribute("onclick", "OnCloseLinkClick();return false;");
            lnkBtnCloseIFrame.setAttribute("href", "");
        }
    }

    //Change the text back to "Close" if changed previously.
    if (lnkBtnCloseIFrame) {
        lnkBtnCloseIFrame.innerHTML = 'Close';
    }

    var lblMsg = $get("ctl00_cphPageContents_lblmsg");
    //Reset the Label Message set earlier
    if (lblMsg && noPageProcess) {
        lblMsg.disabled = "";
        lblMsg.innerHTML = 'Working...Please wait.';
    }

    if (processName.indexOf("BPGID") != -1) {
        //Get the Row-level BPGID and PageInfo information.
        var xDocCurrentRow = loadXMLString("<Root>" + COXML + "</Root>");
        var nodeRow = xDocCurrentRow.getElementsByTagName("Rows")[0];
        var rowBPGID = nodeRow.getAttribute(processName); //Row-Level BPGID
        var pageInfoAttName = "PageInfo";
        var BPBIDIndex = parseInt(trim(processName.replace("BPGID", ""))); //Ex:Get the 2 from BPGID2
        if (!isNaN(BPBIDIndex))//If an index is present
        {
            pageInfoAttName += BPBIDIndex; //Append it to the PageInfo Attribute name.Ex:PageInfo2
        }
        var rowPageInfo = nodeRow.getAttribute(pageInfoAttName); //Row-Level PageInfo

        //Priority is to be given to the Row-level info.
        if (rowPageInfo && rowPageInfo.length > 0) {
            redirectPage = rowPageInfo;
        }
        if (rowBPGID && trim(rowBPGID).length > 0) {
            processBPGID = rowBPGID;
        }
        if (!IsURLValid(redirectPage) || (processBPGID == null || trim(processBPGID) == "" || trim(processBPGID) == "0")) {
            alert("No Business Process is associated with this record!!");
            return;
        }
    }

    //No page Process:Post the data and show the status
    if (noPageProcess) {
        var ProcessStatus = AjaxMethods.SubmitProcess(processBPGID, COXML);
        var ProcessResult = ProcessStatus.value.split("~");
        if (ProcessResult.length == 5) {
            processBPGID = ProcessResult[0];
            redirectPage = ProcessResult[1];
            processName = ProcessResult[2];
            isPopUp = ProcessResult[3];
            COXML = ProcessResult[4];
            setTimeout(function () {
                CallBPCProcess(processBPGID, redirectPage, processName, isPopUp, COXML);
            }, 10);
            return;
        }
        else {
            if (redirectPage == "MsgLine") {
                if (lblMsg) {
                    lblMsg.disabled = "";
                    lblMsg.innerHTML = ProcessStatus.value;
                }
                return;
            }
            else if (redirectPage == "MsgBox") {
                if (lblMsg) {
                    lblMsg.innerHTML = "";
                }
                alert(ProcessStatus.value);
                return;
            }
        }
    }
    var qsPrm = '?';
    if (redirectPage.indexOf(qsPrm) != -1) {
        qsPrm = '&'
    }
    ClearSessionExpireTimeOuts();

    //Explicitly open ShowPDF pages as IsPopUp=1
    if (redirectPage.toUpperCase().indexOf("SHOWPDF") != -1) {
        isPopUp = "1";
    }

    if (isPopUp == "1") {
        //clear nopageprocess message
        if (lblMsg) {
            lblMsg.innerHTML = "";
        }

        //Get the Report Type specified by the RptType radio buttons found in GenProcessEngine.aspx.
        var pageName = redirectPage.split("/");
        redirectPage = "../" + redirectPage + qsPrm + "PopUp=PopUp";
        CommonUI.SetLinkPopUpSession(processBPGID, COXML);
        if (pageName[1].toUpperCase().indexOf("SHOWPDF") != -1 || pageName[1].toUpperCase().indexOf("MAILPDF") != -1) {
            if (typeof (g_PDFOptions) != "undefined") {
                redirectPage = "../Popups/ShowPDF.aspx?PopUp=PopUp&PN=" + g_PDFOptions.PageNumber + "&WM=" + g_PDFOptions.WaterMark + "&PL=" + g_PDFOptions.PageLayout + "&PS=" + g_PDFOptions.PageSize + "&FN=" + g_PDFOptions.FontName + "&PD=" + g_PDFOptions.Password + "&AT=" + g_PDFOptions.Annotations;
            }
            else {
                redirectPage = "../Popups/ShowPDF.aspx?PopUp=PopUp";
            }
            if (rptType && rptType.length > 0) {
                redirectPage += "&RptType=" + rptType;
            }
            var win = window.open(redirectPage, "_blank");
            win.onload = DoSessionExpire;
        }
        else {
            var frameId = ShowIFrame(redirectPage, { reloadParent: procRefresh, beforeClose: AutoFillOnFly });
            SaveToHistory(processBPGID, COXML, redirectPage, frameId);
        }
        g_CurrentPageName = redirectPage;
    }
    else {
        if (processName.indexOf("Success") != 0) {
            UpdateNavHistoryXML(COXML);
        }

        //Posting the page       
        var isPopUp = queryString("PopUp");
        if (isPopUp) {
            //Use the current IFrame window and post the page to that window.
            redirectPage = "../" + redirectPage + qsPrm + "PopUp=PopUp";
            CommonUI.SetLinkPopUpSession(processBPGID, COXML);
            var frameId = RedirectIFrame(redirectPage);
            SaveToHistory(processBPGID, COXML, redirectPage, frameId);
        }
        else {
            redirectPage = "../" + redirectPage;
            $get('hdnBPInfo').value = "<bpinfo><BPGID>" + processBPGID + "</BPGID>" + COXML + "</bpinfo>";
            Redirect(redirectPage);
        }
    }
}

//...................................................................................................//

//If the submit is overidden in the Confirm dialogue then an hdn var is set and posted back.
function ConfirmSubmit(strCnfmText, suggest, cgvucId, cgvuArDescriptionId) {
    strCnfmText = strCnfmText.replace(/~::~/g, " "); //Restore the spaces.
    strCnfmText = unescapeHTML(strCnfmText);
    suggest = suggest.replace(/~::~/g, " "); //Restore the spaces.
    suggest = unescapeHTML(suggest);
    var hdnCnfmSbmt = $get('ctl00_cphPageContents_BtnsUC_hdnCnfmSbmt');
    var proceed = confirm(strCnfmText + "\n\n" + suggest);
    var delTimeOut = 0;
    if (proceed) {
        hdnCnfmSbmt.value = "True";
        //Check for deletions in the childgridview earlier.If so do them explicitly.
        if (cgvucId.length > 0) {
            var hdnRowsToDelete = $get(cgvucId + '_hdnRowsToDelete');
            if (hdnRowsToDelete.value.length > 0) {
                //Update the hdnRowsToDelete to blank to avoid duplication of deletions.
                hdnRowsToDelete.value = '';
                DeleteRow(cgvucId + '_grdVwBranch');
                delTimeOut = 750;
            }
        }

        //Check for deletions in the childgridview earlier.If so do them explicitly for arinvDetail.aspx page
        if (cgvuArDescriptionId.length > 0) {
            var hdnRowsToDelete = $get(cgvuArDescriptionId + '_hdnRowsToDelete');
            if (hdnRowsToDelete.value.length > 0) {
                //Update the hdnRowsToDelete to blank to avoid duplication of deletions.
                hdnRowsToDelete.value = '';
                DeleteRow(cgvuArDescriptionId + '_grdVwBranch');
                delTimeOut = 750;
            }
        }


        //__doPostBack('ctl00$cphPageContents$imgbtnSubmit','');
        setTimeout(function () { $get('ctl00_cphPageContents_imgbtnSubmit').click(); }, delTimeOut);
    }
    else {
        hdnCnfmSbmt.value = "False";
    }
}
//.................................................................................................//
function PreInitEmailHover(BPGID, hdnVarName, sender) {
    var hdnRow = this.$get('ctl00_cphPageContents_BtnsUC_hdnRwToBeModified');
    var rowXML = hdnRow.value;
    var xmlDoc = loadXMLString('<Root>' + rowXML + '</Root>')
    var xmlNode = xmlDoc.getElementsByTagName("Root");
    var xmlRoot = xmlNode[0];
    nodeItem = xmlRoot.childNodes[0];
    var attEmail = nodeItem.getAttribute("Email") || '';
    if (attEmail != '') {
        attEmail = attEmail.split(',').join(';');
    }
    sender.setAttribute('BPGID', BPGID);
    sender.setAttribute('XMLStoreId', hdnVarName);
    sender.setAttribute('MailIDs', attEmail);
}

//...................................................................................................//

//Called when an bottom-level item has been clicked upon in the Main menu.
function MainMenuItemClick(BPGID, redirectPage) {
    RedirectPage(BPGID, redirectPage);
}

//...................................................................................................//

function PopUpPage(BPGID, redirectPage, e) {
    redirectPage = "../" + redirectPage + "?PopUp=PopUp"; //Depth is always one since this popup will be called only form the menu and the menu only exists in pages without depth.
    var bpInfo = '';
    CommonUI.SetLinkPopUpSession(BPGID, bpInfo);
    var frameId = ShowIFrame(redirectPage);
    SaveToHistory(BPGID, bpInfo, redirectPage, frameId);
    return false;
}
//...................................................................................................//

function RedirectPage(BPGID, redirectPage) {
    //Check if the page is in edit mode before redirecting.
    var isInEditMode = false;
    var hdnCurrAction = $get('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
    if (hdnCurrAction && trim(hdnCurrAction.value).length != 0) {
        var action = hdnCurrAction.value;
        if (trim(action) != 'Select') {
            isInEditMode = true;
        }
    }
    var isOk = true;
    if (isInEditMode) {
        isOk = confirm('You have attempted to leave this page. If you have made any changes to the fields without clicking the Submit button, your changes will be lost.  Are you sure you want to exit this page?');
    }

    if (isOk) {
        redirectPage = "../" + GetRedirectPage(redirectPage);
        var bpInfo = "<bpinfo><BPGID>" + BPGID + "</BPGID></bpinfo>";
        AjaxMethods.SetBPEINFO(bpInfo, function (resp) { top.window.location = redirectPage; });
        SaveToHistory(BPGID, '', redirectPage);
    }
}

//...................................................................................................//

function GetRedirectPage(page) {
    return page;
    //    switch(page)
    //    {
    //        case "Common/FullView.aspx":
    //        {
    //            return "Common/GenView.aspx";
    //        }
    //        case "Common/ProcessEngine.aspx":
    //        {
    //            return "Common/GenProcessEngine.aspx";
    //        }
    //        case "Common/ProcessEngine1x.aspx":
    //        {
    //            return "Common/GenProcessEngine.aspx";
    //        }
    //        case "Common/ProcessEngine2x.aspx":
    //        {
    //            return "Common/GenProcessEngine.aspx";
    //        }
    //        default:
    //        {
    //            return page;
    //        }
    //    }
}


//Updates the Navigation History XML when a non-PopUp link is clicked.
//Parameter bpeInfoXML:XML being posted to the next page without the Root Tags.
function UpdateNavHistoryXML(bpeInfoXML) {
    var previousPage;
    var previousPageBPInfo;
    var nodeSelPageRoot;
    if (bpeInfoXML != "") {
        var co = "<Root>" + bpeInfoXML + "</Root>";
        var xCOXMLDoc = loadXMLString(co);
        var nodeCORoot = xCOXMLDoc.getElementsByTagName("Root"); //Root Node            
        nodeSelPageRoot = nodeCORoot[0].firstChild;
        if (nodeSelPageRoot) {
            var pageRowlist = nodeSelPageRoot.getElementsByTagName("RowList");
            var pageRows = pageRowlist[0].getElementsByTagName("Rows");
            pageRows[0].setAttribute("BPAction", "Find");
        }
        var nodeCallingObj = nodeCORoot[0].getElementsByTagName("CallingObject");
        var nodeBPGID = nodeCallingObj[0].firstChild;
        previousPageBPInfo = ConvertToString(nodeBPGID) + ConvertToString(nodeSelPageRoot);

        var nodePageInfo = nodeCallingObj[0].getElementsByTagName("PageInfo");
        previousPage = nodePageInfo[0].firstChild;
    }

    //Setting the hdn Navigation BPInfo
    var navBPInfo = $get('ctl00_cphPageContents_BtnsUC_hdnNavBPInfo');
    if (navBPInfo != null) {
        if (trim(navBPInfo.value).length == 0) {
            navBPInfo.value = "<Root></Root>";
        }
        var NavBPInfoXML = navBPInfo.value;
        var xNavDoc = loadXMLString(NavBPInfoXML);
        var nodeRoot = xNavDoc.getElementsByTagName("Root");
        var linkText = "Link1";
        if (nodeRoot != null) {
            nodeRoot = nodeRoot[0];
            var navRootChildNodes = nodeRoot.childNodes;
            if (navRootChildNodes) {
                var navBPInfoCount = navRootChildNodes.length;
                if (navBPInfoCount > 0)//From 2nd link onwards
                {
                    linkText = "Link" + (navBPInfoCount + 1);
                }
            }
            var nodeLink = xNavDoc.createElement(linkText); //Create Link1 Node
            nodeRoot.appendChild(nodeLink);
            var nodeRedirectPage = xNavDoc.createElement("RedirectPage"); //Create RedirectPage Node
            nodeLink.appendChild(nodeRedirectPage);
            //var newRedirectPagetext=xNavDoc.createTextNode(previousPage);
            nodeRedirectPage.appendChild(previousPage.cloneNode(true));
            var nodeBpInfo = xNavDoc.createElement("bpinfo"); //Create bpinfo Node
            nodeLink.appendChild(nodeBpInfo);
            //var newBpInfotext=xNavDoc.createTextNode(previousPageBPInfo);
            previousPageBPInfo = "<Root>" + previousPageBPInfo + "</Root>";
            var newBpInfotext = loadXMLString(previousPageBPInfo).getElementsByTagName("Root")[0];
            nodeBpInfo.appendChild(newBpInfotext.firstChild.cloneNode(true));
            nodeBpInfo.appendChild(newBpInfotext.firstChild.nextSibling.cloneNode(true));

            //Convert the Document back to a string and update
            var updatedNavBPInfo = ConvertToString(nodeRoot);
            $get("ctl00_cphPageContents_BtnsUC_hdnNavBPInfo").setAttribute("value", updatedNavBPInfo);
        }
    }
}

//...................................................................................................//

//Called when the form-level process link Icons(VendorSelect & CustomerSelect) are clicked upon in the page.
function OnFormBPCIconClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp) {
    OnFormBPCLinkClick(processBPGID, redirectPage, bpeInfoClientID, processName, isPopUp);
}

//...................................................................................................//

function ShowDetailedView(BPGID, pageSize) {
    var redirectPage = "../Common/DetailedView.aspx";
    var bpeInfo = GenerateBPInfo(BPGID, '1', pageSize, '', '');
    AjaxMethods.ResetLinkPopUpSession(bpeInfo);
    var frameId = ShowIFrame(redirectPage);
    SaveToHistory(BPGID, '', redirectPage, frameId);
    return false; //Don't hit the server.
}

//...................................................................................................//

function GenerateBPInfo(processBPGID, pgNo, pgSize, sortCol, sortOrder) {
    return "<bpinfo><BPGID>" + processBPGID + "</BPGID><Gridview><Pagenumber>" + pgNo
            + "</Pagenumber><Pagesize>" + pgSize + "</Pagesize><Sortcolumn>" + sortCol
            + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder></Gridview></bpinfo>";
}

//...................................................................................................//

//Event handler for Escape key press event.
var isSameEvent = false; //To avoid calling of the handler multiple times for a single key press event.
function OnKeyPress(args) {
    if (args.keyCode == Sys.UI.Key.esc && !isSameEvent) {
        isSameEvent = true;
        setTimeout('isSameEvent=false', 100);
        HideFrame();
    }
}

//...................................................................................................//

//Closes the Top-most frame(if exists) in the page.
//Returns the most active Window object.
function CloseTopMostFrame() {
    var xx = top.$get('iframePage');
    var yy;
    while (xx && xx.src != '') {
        yy = xx;
        var objThisDoc = xx.contentWindow || xx.contentDocument;
        if (objThisDoc.document) {
            objThisDoc = objThisDoc.document;
        }
        xx = objThisDoc.getElementById('iframePage');
    }

    var yyDoc = yy.contentWindow || yy.contentDocument;
    if (yyDoc.document) {
        yyDoc = yyDoc.document;
    }
    var yyParent = yy.contentWindow.parent;
    yyParent.DisplayPopUp(false);
    yy.src = '';
    return yyParent;
}

//.................................................................................................//

function GetActiveFrame() {
    var xx = top.$get('iframePage');
    var yy = top;
    while (xx && xx.src != '') {
        yy = xx;
        var objThisDoc = xx.contentWindow || xx.contentDocument;
        if (objThisDoc.document) {
            objThisDoc = objThisDoc.document;
        }
        xx = objThisDoc.getElementById('iframePage');
    }
    if (yy.contentWindow && (yy.contentWindow.location != top.window.location)
            && (yy.contentWindow.location != 'about:blank'))
        return yy.contentWindow;
    else
        return yy;
}

//...................................................................................................//

//Set attributes for all links in the parent grid to override the page-exit confirmation.
function SetParentGridLinks() {
    var parentGrid;
    if ($get('ctl00_cphPageContents_GVUC_grdVwContents')) {
        parentGrid = $get('ctl00_cphPageContents_GVUC_grdVwContents');
    }
    else if ($get('ctl00_cphPageContents_PCGVUC_grdVwContents')) {
        parentGrid = $get('ctl00_cphPageContents_PCGVUC_grdVwContents');
    }

    if (parentGrid) {
        var arrGridLinks = parentGrid.getElementsByTagName('A');
        for (var cntr = 0; cntr < arrGridLinks.length; cntr++) {
            arrGridLinks[cntr].onmouseover = function () { g_temp = false; }
            arrGridLinks[cntr].onmouseout = function () { g_temp = true; }
        }
    }
}

//...................................................................................................//

//Gets the client's screen resolution(Width x Height).
function GetScrnResolution() {
    return window.screen.width + "x" + window.screen.height;
}

//...................................................................................................//

//Opens Print PopUp.
var newWin = null;
function OpenPrintPopup() {
    var strType = "fixed";
    var strOptions = "";

    if (newWin != null && !newWin.closed)
        newWin.close();

    if (strType == "fixed")
        strOptions = "status,height=200,width=300";

    var strURL = "../PopUps/PrintPopUp.aspx";
    newWin = window.open(strURL, 'newWin', strOptions);
    newWin.focus();
}

//...................................................................................................//

//Check SOXApproval
function SOXApproval(bpeInfoClientID) {
    bpeInfoClientID = bpeInfoClientID + "_hdnGVBPEINFO";
    var COXML = $get(bpeInfoClientID).value;
    //Load the XML into a DOM object
    var xDocCurrentRow = loadXMLString("<Root>" + COXML + "</Root>");
    var nodeRow = xDocCurrentRow.getElementsByTagName("Rows")[0];
    var approvedStatus = nodeRow.getAttribute("IsApproved");

    var message = "";
    if (approvedStatus == "0") {

        message = "Do you want to approve the transaction?";
    }
    if (approvedStatus == "1") {

        message = "Do you want to cancel the approval?";
    }
    return confirm(message);
}

//...................................................................................................//

//Check SOXApproval by Image Click
function SoxApprovedStatus(IsApproved) {
    var message = "";
    if (IsApproved == "0") {

        message = "Do you want to approve the transaction?";
    }
    if (IsApproved == "1") {

        message = "Do you want to cancel the approval?";
    }
    return confirm(message);
}

//...................................................................................................//
//var objSoxQTip; 
function SoxApprovalToolTipShow(SoxMessage, IsApproved, ObjClientId) {
    var tableInnerText = "<table border='0' cellpading='0' cellspacing='0'>";

    if (trim(SoxMessage) != '') {
        tableInnerText += "<tr><td>" + SoxMessage + "</td></tr>";
        tableInnerText += "<tr><td>&nbsp;</td></tr>";
    }

    if (IsApproved == "0") {
        tableInnerText += "<tr><td>Please click to approval</td></tr>";
    }
    if (IsApproved == "1") {
        tableInnerText += "<tr><td>Please click to disapproval</td></tr>";
    }
    tableInnerText += "</table>";


    var objTarget = $get(ObjClientId);
    if (objTarget != null) {
        var ttContent;
        ttContent = { text: tableInnerText, title: { text: '<span style="padding:2px">Approval Status</span>'} };

        var ttPosition = { corner: { target: 'topLeft', tooltip: 'bottomRight' }, adjust: { screen: true} };
        var objSoxQTip = jQuery(objTarget).qtip(
            {
                content: ttContent,
                position: ttPosition,
                //style: { tip:'bottomRight',padding: '5px 5px',name: 'grid', border: { width: 2, radius: 6 } },
                style: { tip: 'bottomRight', padding: '5px 5px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A'} },
                show: { ready: true },
                hide: { when: 'mouseout', fixed: true }
            });
    }
}

//...................................................................................................//
////Alerts the user on the event of leaving the page
////Should come from the server-side based on the XML(A hidden varible on the server-side should be set to the desired value)
////This variable can be set otherwise on the client-clicks on respective postback triggers.(Most probably BtnsUC)
//var g_temp=true;//Used as additional verifier whether to show alert or not.In the events where alert is not desired set this variable to false.
//window.onbeforeunload=function(e)
//{
//    var needToConfirm = $get('ctl00_cphPageContents_BtnsUC_hdnNeedToConfirmExit');
//    if(needToConfirm && needToConfirm.value == "True" && g_temp && top.g_temp)
//    {
//        e.returnValue = "You have attempted to leave this page. If you have made any changes to the fields without clicking the Submit button, your changes will be lost.  Are you sure you want to exit this page?";
//        DisableConfirm();
//        needToConfirm.value="False";
//        top.g_temp=false;//Avoid multiple confirmations when in an IFrame view.
//        
////        //setTimeout("alert('hi from setTimeout()');",500);
////        // setTimeout() and setInterval() aren't called when ok is clicked in 
////        // IE5-6/Win, but is called in IE7 when the time is short, but not when 
////        // it's longer, like 500 (a half second).
////        window.unloadTimer = setInterval("OnConfirmExitCancel();clearInterval(window.unloadTimer);",500);
////        window.onunload = function() { clearInterval(window.unloadTimer);}
////        return 'onbeforeunload testing';
//        
//        //Launch a timer that will check whether there is any history item which is still loading.
//        //if found remove that item and cancel the timer.
////        window.unloadTimer = setInterval("var done=OnConfirmExitCancel();if(done){clearInterval(window.unloadTimer);}",500);
//    }
//}

function OnConfirmExitCancel() {
    return RemoveLoadingItem(); //Function in FrameManager.js
}

//Resets the Session BPINFO variable on page unload
function ResetSession() {
    //Reset the Session-BPInfo to the BpInfo of the topDocument i.e overriding all the IFrames.
    var bpInfoContainer = top.$get('ctl00_cphPageContents_parentBPInfo');
    if (bpInfoContainer == null) {
        bpInfoContainer = top.$get('ctl00_cphPageContents_BtnsUC_parentBPInfo');
    }
    if (bpInfoContainer && trim(bpInfoContainer.value).length > 0) {
        if (typeof (AjaxMethods) == 'object') {
            AjaxMethods.ResetLinkPopUpSession(bpInfoContainer.value);
        }
        else if (typeof (CommonUI) == 'object') {
            CommonUI.ResetLinkPopUpSession(bpInfoContainer.value);
        }
    }
}

//...................................................................................................//

function DisableConfirm() {
    g_temp = false;
    setTimeout('EnableConfirm()', '100');
}

//...................................................................................................//

function EnableConfirm() {
    g_temp = true;
}

if (typeof DOMParser == "undefined") {
    DOMParser = function () { }

    DOMParser.prototype.parseFromString = function (str, contentType) {
        if (typeof ActiveXObject != "undefined") {
            var d = new ActiveXObject("MSXML.DomDocument");
            d.loadXML(str);
            return d;
        } else if (typeof XMLHttpRequest != "undefined") {
            var req = new XMLHttpRequest;
            req.open("GET", "data:" + (contentType || "application/xml") +
                         ";charset=utf-8," + encodeURIComponent(str), false);
            if (req.overrideMimeType) {
                req.overrideMimeType(contentType);
            }
            req.send(null);
            return req.responseXML;
        }
    }
}

//...................................................................................................//

//Hides the top-most overlaying pop-up if present and returns the result of that operation as boolean
function HideIPagePopUp() //**Not in use
{
    var objPopUp = $find('mpePagePopUpBehaviourID');
    if (objPopUp && objPopUp._foregroundElement.style.display == '') {
        objPopUp.hide();
        setTimeout('HideIPagePopUp()', 200); //Pop-up reappearing even after calling hide().So call the method once again.
        return true;
    }
    else {
        return false;
    }
}

//...................................................................................................//

//Validate amount field 2 digits
function ValidateDecimals(Amount) {
    var splitwords = Amount.split(".");
    var Status = 0;
    if (splitwords.length > 2) {
        Status = 2;
    }
    if (splitwords.length > 1) {
        if (splitwords[1].length > 2) {
            Status = 2;

        }
    }
    if (Status == 2) {
        return false;
    }
    else {
        return true;
    }
}

//...................................................................................................//

//Called when the MasterPage-level process links are clicked upon in the page.-Modification Request functionality.
function OnMasterPgBPCLinkClick(processBPGID, redirectPage, formBPGID, isPopUp, hdnVarName) {
    var COXML;
    ChangeCloseLinkText('', this);
    var hdnCurrentAction = $get(hdnVarName).value;
    var callingBPGID = formBPGID;
    if (hdnCurrentAction && hdnCurrentAction.value.length > 0) {
        callingBPGID = hdnCurrentAction;
    }
    COXML = "<CallingObject><BPGID>" + callingBPGID + "</BPGID></CallingObject>";
    //Display the page in a pop-up or in a window accordingly.
    if (isPopUp == "1") {
        //        var iFramePage=$get("iframePage");
        //        iFramePage.height=g_IFrameHeight;
        //        iFramePage.width=g_IFrameWidth;
        //        DisplayPopUp(true);
        redirectPage = "../" + redirectPage + "?PopUp=PopUp";
        CommonUI.SetLinkPopUpSession(processBPGID, COXML);
        ShowIFrame(redirectPage, { isModal: true });
        //        iFramePage.src = redirectPage;  
    }
    else {
        redirectPage = "../" + redirectPage;
        $get('hdnBPInfo').value = "<bpinfo><BPGID>" + processBPGID + "</BPGID>" + COXML + "</bpinfo>";
        Redirect(redirectPage);
    }
}

//...................................................................................................//

//Called to reset the parent BPInfo to the page after closing the child(Page PopUP).
function OnCloseLinkClick() {
    SetScrollBars(true);
    //Killing Session For Upload Text
    if (g_CurrentPageName != null) {
        var currentPage = g_CurrentPageName.split('/');
        var currentPageNames = currentPage[2].split('?');
        if (currentPageNames[0].toUpperCase().indexOf("UPLOADTEXT") != -1) {
            AjaxMethods.KillSession('<%=Session["Row"]%>');
        }
    }

    var parentBPInfo = "";
    var BtnsUCExists = false;
    if ($get('ctl00_cphPageContents_BtnsUC_parentBPInfo')) {
        BtnsUCExists = true;
    }
    //If Btns UC does not exists it is Process Engine page  
    var bpInfoContainer;
    if (!BtnsUCExists) {
        bpInfoContainer = $get('ctl00_cphPageContents_parentBPInfo');
    }
    else {
        bpInfoContainer = $get('ctl00_cphPageContents_BtnsUC_parentBPInfo');
    }
    bpInfoContainer = bpInfoContainer || $get('ctl00_cphPageContents_hdnBPInfo'); //For pages with jqGrid 
    if (bpInfoContainer) {
        parentBPInfo = bpInfoContainer.value;
    }
    if (trim(parentBPInfo).length > 0) {
        if (typeof (AjaxMethods) == 'object') {
            AjaxMethods.ResetLinkPopUpSession(parentBPInfo);
        }
        else if (typeof (CommonUI) == 'object') {
            CommonUI.ResetLinkPopUpSession(parentBPInfo);
        }
    }



    /*AutoFill New Entry Textboxes loading in parent page*/
    var eyeframe = $get('iframePage');
    if (eyeframe) {
        var eyeframedoc = eyeframe.contentWindow ? eyeframe.contentWindow.document : eyeframe.contentDocument;
        var hdnAutoFill = eyeframedoc.getElementById('ctl00_cphPageContents_hdnAutoFillNewEntry');
        if (hdnAutoFill) {
            hdnAutoFill = hdnAutoFill.value;
            var hdnInfo = hdnAutoFill.split(';');
            var ObjName = hdnInfo[0];
            var ObjValue = hdnInfo[1];
            if ((parent.$get(ObjName) != null) && (ObjValue != null)) {
                var txtParent = parent.$get(ObjName);
                txtParent.value = ObjValue;
                //Trigger the Search Event to populate the parent of the clone textbox.
                jQuery(txtParent).search();

            }
            else { //nested iframe case
                if ($get(ObjName) != null) {
                    var txtParent = $get(ObjName);
                    txtParent.value = ObjValue;
                    //Trigger the Search Event to populate the parent of the clone textbox.
                    jQuery(txtParent).search();
                }
            }
        }
    }

    //Hide loading image
    HideParentProgress();
    var iframe = this.window.$get("iframePage");
    if (iframe != null) {
        //global variable to load help iframe page
        g_IFramePage = "";
        //        //Reset Default height and widths of popups
        //        iframe.width = g_IFrameWidth;
        //        iframe.height = g_IFrameHeight;
        iframe.src = "";
    }
    DisplayPopUp(false);
}

//...................................................................................................//    
function AutoFillOnFly(frameObj) {
    /*AutoFill New Entry Textboxes loading in parent page*/
    if (frameObj && typeof (frameObj[0].contentDocument) == 'object') {
        //var eyeframedoc = frameObj[0].contentWindow ? frameObj[0].contentWindow.document: frameObj[0].contentDocument;
        var eyeframedoc = frameObj[0].contentDocument;
        var hdnAutoFill = eyeframedoc.getElementById('ctl00_cphPageContents_hdnAutoFillNewEntry');
        if (hdnAutoFill) {
            hdnAutoFill = hdnAutoFill.value;
            var hdnInfo = hdnAutoFill.split(';');
            var ObjName = hdnInfo[0];
            var ObjValue = hdnInfo[1];
            if ((parent.$get(ObjName) != null) && (ObjValue != null)) {
                var txtParent = parent.$get(ObjName);
                txtParent.value = ObjValue;
                //Trigger the Search Event to populate the parent of the clone textbox.
                jQuery(txtParent).search();

            }
            else { //nested iframe case
                if ($get(ObjName) != null) {
                    var txtParent = $get(ObjName);
                    txtParent.value = ObjValue;
                    //Trigger the Search Event to populate the parent of the clone textbox.
                    jQuery(txtParent).search();
                }
            }
        }
    }
}

//.............................Client-Side Session Management......................................................................//  
var g_TimeOut;
var g_TimerIDSessionExpire; //TimerId for Session Expiry
var g_TimerIDSessionAlert; //TimerId for Session Alert
var g_TimerIDDisplayCountDown; //TimerId for DisplayCountDown

//Resets to the Timers to their passed values from the server.
function DoSessionExpire() {
    //    if (typeof (g_TimeOut) == 'undefined' || g_TimeOut == 0) {
    //        return;
    //    }

    ////    Redirect to SessionExpire page.
    //    var alertBefore = parseInt(top.g_TimeOut - (top.seconds * 1000));
    //    ClearSessionExpireTimeOuts();
    //    top.g_TimerIDSessionExpire = top.setTimeout(function () { g_temp = false; top.location = '../Common/SessionExpire.aspx'; }, top.g_TimeOut);
    //    top.g_TimerIDSessionAlert = top.setTimeout(function () { top.ShowSessionExpireAlert(); }, alertBefore);

    //Reset the session on the server.
    if (top.g_TimerIDSessionExpire) {
        top.clearInterval(top.g_TimerIDSessionExpire);
        Log("Previous Interval cleared");
    }

    if (typeof (top.g_TimeOut) != 'undefined' && top.g_TimeOut != 0) {
        Log("Handling session expiry. New interval created at : " + getTime());
        top.g_TimerIDSessionExpire = top.setInterval(top.SessionReset, top.g_TimeOut - 10000);
    }
    else {
        Log("top.g_TimeOut is null.");
    }
}

function SessionReset() {
    var wRequest = new Sys.Net.WebRequest();
    wRequest.set_url("../Common/SessionKeepAlive.aspx");
    wRequest.set_httpVerb("POST");
    wRequest.add_completed(SessionReset_Callback);
    wRequest.set_body();
    wRequest.get_headers()["Content-Length"] = 0;
    wRequest.invoke();
}

//Logs a string message to Firebug's console.
function SessionReset_Callback(objWreq, args) {
    Log("Session has been automatically refreshed on the server : " + getTime());
}

function Log(str) {
    if (typeof (console) != "undefined" && console != null) {
        console.log(str);
    }
}

function getTime() {
    return (new Date()).toLocaleTimeString();
}

//Clears all timers running for Session Expire, it's alert and the alert DIV for every Async PostBack.
function ClearSessionExpireTimeOuts() {
    var currWin = top;
    if (currWin.g_TimerIDSessionExpire) {
        currWin.clearTimeout(currWin.g_TimerIDSessionExpire);
    }
    if (currWin.g_TimerIDSessionAlert) {
        currWin.clearTimeout(currWin.g_TimerIDSessionAlert);
    }
    if (currWin.g_TimerIDDisplayCountDown) {
        currWin.clearTimeout(currWin.g_TimerIDDisplayCountDown);
    }
    var divCreatedBefore = currWin.$get('divSessionExpire');
    if (divCreatedBefore) {
        currWin.jQuery(divCreatedBefore).remove();
    }
}

//...................................................................................................//

//Displays an alert to the user that his/her session is about to expire.
function ShowSessionExpireAlert() {
    var divWidth = 300;
    var div = top.document.createElement('div');
    div.style.zIndex = 100002009;
    div.id = 'divSessionExpire';
    div.style.display = 'none';
    div.style.width = divWidth + 'px';
    div.style.height = '100px';
    div.innerHTML = '<center><b>Your session is about to expire in <div id="divCountDown" style="display:inline;text-decoration:underline"></div> seconds. Press Ok to refresh the page again.<b></center>';
    top.document.body.appendChild(div);
    top.jQuery(div).dialog({
        title: 'Session Expire Alert!!',
        autoOpen: true,
        bgiframe: false,
        width: 300,
        //                               height:100,
        modal: true,
        show: 'clip',
        //		                         hide:'clip',
        draggable: false,
        resizable: false,
        closeOnEscape: true,
        buttons: { "Ok": function () { top.jQuery(this).dialog("close"); top.RefreshPage(); } }
    });
    top.DisplayCountDown();
}

var seconds = 5000;
//Facilitates a count down timer for the above alert functionality.
function DisplayCountDown() {
    top.g_TimerIDDisplayCountDown = top.setTimeout("top.DisplayCountDown()", 1000);
    top.$get('divCountDown').innerHTML = '<b>' + top.seconds + '</b>';
    top.seconds--;
    if (top.seconds == -1) {
        top.window.clearTimeout(top.g_TimerIDDisplayCountDown);
        var divCreatedBefore = top.$get('divSessionExpire');
        if (divCreatedBefore) {
            top.document.body.removeChild(divCreatedBefore);
        }
    }
}

//...................................................................................................//

//Renders the full blown view of the Menu Panel for a given Panel.
function RenderFullMenuView(dataPath) {
    try {
        RenderThePanel(dataPath);
    }
    catch (error) {
        if (g_IsDebugging) {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : RenderFullMenuView()\nFileName : ExplodedView.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}

//...................................................................................................//

//Called when the form-level Navigation links are clicked upon in the page.
function OnNavLinkClick(linksCnt) {
    var isPopUp;
    var NavBPInfoXML = $get('ctl00_cphPageContents_BtnsUC_hdnNavBPInfo').value;
    //Pop Up
    if (!NavBPInfoXML) {
        alert('Testing Purpose:OnNavLinkClick()-PopUp case');
        NavBPInfoXML = parent.$get('ctl00_cphPageContents_BtnsUC_hdnNavBPInfo').value;
        if (NavBPInfoXML) {
            if (linksCnt == "Link1") {
                isPopUp = "TRUE";
            }
        }
    }

    var xNavDoc = loadXMLString(NavBPInfoXML);
    var nodeRoot = xNavDoc.getElementsByTagName("Root");
    if (nodeRoot) {
        nodeRoot = nodeRoot[0];
        if (nodeRoot) {
            var nodeNavRow = nodeRoot.getElementsByTagName(linksCnt);
            var redirectPage;
            if (nodeNavRow) {
                nodeNavRow = nodeNavRow[0];
                //Get the Page Info(Redirect Page) from the row's XML
                var nodeRedirectPage = nodeNavRow.getElementsByTagName("RedirectPage");
                if (nodeRedirectPage) {
                    nodeRedirectPage = nodeRedirectPage[0];
                    redirectPage = "../" + nodeRedirectPage.firstChild.nodeValue;
                }
                //Get the BPInfo of Redirect Page from the selected row's XML
                var currentNavBPinfo = nodeNavRow.getElementsByTagName("bpinfo");
                if (currentNavBPinfo) {
                    currentNavBPinfo = currentNavBPinfo[0];
                    //Assigning the BpInfo of Redirect Page to hdn variable in BtnsUC
                    //$get('hdnBPInfo').value = currentNavBPinfo[0].value;             
                    $get('hdnBPInfo').setAttribute("value", ConvertToString(currentNavBPinfo));
                }

                //Pop the current navigation link before redirecting and update the hdnNavBPInfo variable.
                nodeRoot.removeChild(nodeNavRow);
                $get('ctl00_cphPageContents_BtnsUC_hdnNavBPInfo').value = ConvertToString(nodeRoot);
            }
            //To redirect and show like normal page.
            Redirect(redirectPage);
        }
    }
}


//...................................................................................................//

function ChangeCloseLinkText(newText, win) {
    if (typeof (win) === 'undefined') {
        win = parent;
    }
    var closeBtnID = 'lnkBtnCloseIFrame';
    var targetCloseLink = win.$get('ctl00_' + closeBtnID);
    if (targetCloseLink == null) {
        targetCloseLink = win.$get(closeBtnID); //No Master no BtnsUC
    }
    if (targetCloseLink) {
        if (document.all) {
            targetCloseLink.href = '#';
            var strOnClick = targetCloseLink.onclick;
            targetCloseLink.onclick = function () { win.OnCloseLinkClick(); return false; }
        }
        targetCloseLink.innerHTML = "Close " + newText;
    }
    if (typeof (UpdateTitle) == 'function')
        UpdateTitle(newText);
}

//...................................................................................................//

//Closes the Exploded View present in the Master Page(Pages other than DashBoard).
function CloseExpView() {
    //$find('mpeExpViewBehavID').hide();
    DisplayPopUp(false, 'ctl00_pnlExplodedView');
}

//...................................................................................................//

function GetSpanChild(parentNode) {
    x = parentNode.firstChild;
    while (x.nodeType != 1) {
        x = x.nextSibling;
    }
    return x;
}

//...................................................................................................//

/*Displays a update progress on iframe opened popups screen*/
function ShowIframeProgress() {
    var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
    //Add a div in the middle of the page on top of the semi-transparent layer.
    if ($get('box') == null)//Check whether postback has completed within the ShowProgress Delay.
    {
        //var cdnImagesPath=$get('ctl00_hdnImagesCDNPath').value;
        var div = document.createElement('div');
        div.style.zIndex = 100002009;
        div.id = 'box';
        div.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
        div.style.top = '200px';
        div.style.left = (width / 2) - 50 + 'px';
        div.innerHTML = '<img alt="Please wait.." src="' + g_cdnImagesPath + 'Images/loading-lajit.gif" />';
        document.body.appendChild(div);
    }
}

//...................................................................................................//

//hide progress on iframe for exclusively for print
function HideIframeProgress() {
    var divLayer = $get('layer');
    var divImage = $get('box');
    if (divLayer) {
        document.body.removeChild(divLayer);
    }
    if (divImage) {
        document.body.removeChild(divImage);
    }
}

//...................................................................................................//

//hide progress on iframe opened regular pages.
function HideParentProgress() {
    var divLayer = parent.$get('layer');
    var divImage = parent.$get('box');
    if (divLayer) {
        parent.document.body.removeChild(divLayer);
    }
    if (divImage) {
        parent.document.body.removeChild(divImage);
    }

    if ((!divLayer) && (!divImage)) {
        //both divs are  null
        HideIframeProgress();
    }


    //To Set the Width of Child Grid 
    var currentPage = window.location + ""; //Cast the object to string.
    if (currentPage.toUpperCase().indexOf('COMMERCIAL.ASPX') != -1) {
        SetChildHeaderWidth();
    }
}

//...................................................................................................//

//Returns the CallingObject XML string constructed with the passed parameters.
function GetCallingObjectXML(trxID, trxType, formBPGID, caption, navigatePage) {
    return "<CallingObject><BPGID>" + formBPGID + "</BPGID><TrxID>"
        + trxID + "</TrxID><TrxType>" + trxType + "</TrxType><PageInfo>" + navigatePage
        + "</PageInfo><Caption>" + caption + "</Caption></CallingObject>";
}

//...................................................................................................//

function OnAppKeyDown(eventname) {
    var version = navigator.appVersion;
    //    try{
    var keyCode = (window.event) ? event.keyCode : eventname.keyCode;
    switch (keyCode) {
        //        case 39://Right Arrow        
        //        {        
        //            IteratePanel('forward');//Function in ExplodedView.js        
        //            return true;        
        //        }        
        //        case 37://Left Arrow        
        //        {        
        //            IteratePanel('backward');//Function in ExplodedView.js        
        //            return true;        
        //        }        
        case 120: //F9
            {
                //top.MaximiseFrame(window.event||eventname);
                Maximise(window.event || eventname);
                break;
            }
        default:
            {
                return true;
            }
    }
    //    }
    //    catch(e){console.log(e);}
}


////Global variable to store the previous dimensions.
//function MaximiseFrame(e)
//{
//    try{
//    if(e.preventDefault)e.preventDefault();
//    else e.returnValue=false;
//    //Get the base window's dimensions.
//    if(typeof(jQuery.maximised)=="undefined" || jQuery.maximised==false)
//    {
//        //Maximise.
//        var baseWidth=jQuery(this).width();
//        var baseHeight=jQuery(this).height();
//        var current=$get('iframePage');
//        var winParent=this;
//        var depth=1;
//        while(current&&current.src.length>0)
//        {
//            var currDims=new Object();//Store the existing values.
//            var setWidth=baseWidth-(depth*8);//The amount to which maximise should happen.
//            var setHeight=baseHeight-(depth*18);
//            var jqWin=jQuery(current);
//            currDims.width=jqWin.width();//Store the current dimensions
//            currDims.height=jqWin.height();
//            
//            //Set the Frame Dimensions.
//            if(setWidth!=0&&setHeight!=0) {
//                jqWin.width(setWidth).height(setHeight);
//            }
//            else{alert('Dims are zero!!!');};
//            
//            current=winParent.frames['iframePage'];    
//            //Update the global variables so that inner Iframes take into consideration these values.
//            current.g_IFrameWidth=top.jQuery('#iframePage').width();
//            current.g_IFrameHeight=top.jQuery('#iframePage').height();
//            
//            setWidth = setWidth-56;//Deduct the BtnsUC and spacer widths.
//            
//            //Set the Dialog's position.
//            var winDialog=winParent.jQuery('#ctl00_pnlPagePopUp,#pnlPagePopUp').parent();
//            currDims.top=winDialog.css('top');
//            currDims.left=winDialog.css('left');
//            winDialog.css('top','0px').css('left','0px');
//            
//            if(typeof(current.jQuery)=='function')
//            {
//                //ChildGrid.
//                var cgvuc=current.jQuery('#ctl00_cphPageContents_CGVUC_pnlGVBranch');
//                currDims.CGWidth=cgvuc.width();
//                cgvuc.css('width',setWidth-18);//-3:To avoid overflow scrollbars.
//                
//                var jqGrid=current.jQuery('#tblGrid');
//                if(jqGrid.size()>0) {
//                    var jqChange=setWidth-jqGrid.getGridParam('width')-22;
//                    currDims.jqChange=jqChange;
//                    jqGrid.setGridWidth(jqGrid.getGridParam('width')+jqChange,true);
//                }
//            }
//            winParent=current;
//            if(current.document) {
//                current=current.document.getElementById('iframePage');
//            }
//            else {
//                current=null;
//            }
//            this.jQuery['Frame'+depth]=currDims;//Store the dimensions in a global variable.
//            depth++;
//        }
//        jQuery.maximised=true;
//    }
//    else
//    {
//        //Minimise.
//        var baseWidth=jQuery(this).width();
//        var baseHeight=jQuery(this).height();
//        var current=$get('iframePage');
//        var winParent=this;
//        var depth=1;
//        while(current&&current.src.length>0)
//        {
//            var currDims=this.jQuery['Frame'+depth];//Retreive the previous values.
//            if(!currDims) {
////                alert('Current Dims not defined.');
////                jQuery.maximised=false;
////                MaximiseFrame(e); 
////                return;
//                var parentDims=this.jQuery['Frame'+parseInt(depth-1)];
//                currDims=new Object();
//                currDims.width=g_IFrameWidth-(depth*g_IFDec.Width);
//                currDims.height=g_IFrameHeight-(depth*g_IFDec.Height);
//                currDims.top=4;
//                currDims.left=8;
//            }
//            var jqWin=jQuery(current);
//            if(currDims&&currDims.width!=0&&currDims.height!=0) {
//                jqWin.width(currDims.width).height(currDims.height);
//            }
//            
//            current=winParent.frames['iframePage'];    
//            current.g_IFrameWidth=currDims.width;
//            current.g_IFrameHeight=currDims.height;
//            
//            var winDialog=winParent.jQuery('#ctl00_pnlPagePopUp,#pnlPagePopUp').parent();
//            winDialog.css('top',currDims.top);
//            winDialog.css('left',currDims.left);

//            if(typeof(current.jQuery)=='function')
//            {
//                //ChildGrid.
//                var cgvuc=current.jQuery('#ctl00_cphPageContents_CGVUC_pnlGVBranch');
//                cgvuc.css('width',currDims.CGWidth);
//                
//                if(currDims.jqChange) {
//                    var jqGrid=current.jQuery('#tblGrid');
//                    jqGrid.setGridWidth(jqGrid.getGridParam('width')-currDims.jqChange,true);
//                }
//            }
//            
//            winParent=current;
//            if(current.document) {
//                current=current.document.getElementById('iframePage');
//            }
//            else {
//                current=null;
//            }
//            jQuery[depth]=currDims;
//            depth++;
//        }
//        
//        jQuery.maximised=false;
//    }
//    }catch(e){
//        alert(e.message);
//    }
//    
//}

//...................................................................................................//

//Left Panel ShortCut link click event
function OnShrtCutLinkClick(redirectPage, BPGID) {
    redirectPage = "../" + redirectPage;
    $get('hdnBPInfo').value = "<bpinfo><BPGID>" + BPGID + "</BPGID></bpinfo>";
    Redirect(redirectPage);
}

//...................................................................................................//

function OnHomeImgClick() {
    //Redirect to DashBoard if not in DashBoard.
    var currentPage = window.location + ""; //Cast the object to string.
    if (currentPage.toUpperCase().indexOf('DASHBOARD.ASPX') == -1) {
        window.location = "../Common/DashBoard.aspx";
    }
    else {
        $get('ctl00_cphPageContents_pnlExpView').style.height = "0px";
        $get('ctl00_cphPageContents_pnlExpView').style.visibility = "hidden";
        $get('ctl00_cphPageContents_pnlExpViewContent').style.height = "0px";
        $get('ctl00_cphPageContents_pnlExpViewContent').style.visibility = "hidden";
        $get('imgBtnUpdateShrtCuts').style.visibility = "hidden";
    }
}

//...................................................................................................//

function SetCompanyRole(RoleMode) {
    var IndexValue = $get('ctl00_ddlEntity').selectedIndex;
    var SelectedDataValue = $get('ctl00_ddlEntity').options[IndexValue].value;
    var RoleCompanyID;
    var UserRoleID;

    if (SelectedDataValue != null) {
        var RoleSelected = SelectedDataValue.split("~");
        if (RoleMode == "SET") {
            RoleCompanyID = RoleSelected[0];
            UserRoleID = RoleSelected[1];
        }
        else if (RoleMode == "REMOVE") {
            RoleCompanyID = "";
            UserRoleID = "";
        }
    }
    var RoleResult = AjaxMethods.SubmitCompanyRole(RoleCompanyID, UserRoleID);
    alert(RoleResult.value);
}

//...................................................................................................//

function OpenHelpFrame(isPopUp, redirectPage) {

    if (isPopUp == "1") {
        popWin = window.open(redirectPage, 'FlvPlayer', 'left=100,top=100,width=800,height=500,location=0,menubar=0,toolbar=0,status=0, scrollbars=0, resizable=0');
        popWin.focus();
        return false;
    }
}

function HelpXcoor() {
    //var x = (screen.availWidth / 2) - (document.body.clientWidth / 2);
    var x = (screen.availWidth / 2) + 50;
    x = (x > 0) ? x : 0;
    return x;
}
function HelpYcoor() {
    var y = ((screen.availHeight - 60) / 2) - (document.body.clientHeight / 2);
    y = (y > 0) ? y : 10;  //10
    return y;
}



//...................................................................................................//

function OpenHelpModalPopup(isPopUp, redirectPage, e) {
    if (e) e.stopPropagation();
    if (isPopUp == "1") {
        UpdateBpInfo(this); //FrameMngr.js
        var pageName = redirectPage.split("/");
        var currentPageNames = pageName[1];
        g_IFramePage = currentPageNames;
        var dialogOpts = { reloadParent: false, isModal: true };
        if (currentPageNames.toUpperCase().indexOf("HELP.ASPX") != -1) {
            dialogOpts.width = 464;
            dialogOpts.height = 524;
        }
        redirectPage = "../" + redirectPage + "?PopUp=PopUp&depth=1";
        ShowIFrame(redirectPage, dialogOpts);
    }
    else {
        window.open("../" + redirectPage, "_blank");
    }
}

//...................................................................................................//

function backButtonOverride() {
    // Work around a Safari bug,that sometimes produces a blank page
    setTimeout("backButtonOverrideBody()", 1);
}

//...................................................................................................//

function backButtonOverrideBody() {
    // Works if we backed up to get here
    try {
        history.forward();
    }
    catch (e) {
        //Ignore
    }
    // Every quarter-second, try again. The only
    // guaranteed method for Opera, Firefox,
    // and Safari, which don't always call
    // onLoad but *do* resume any timers when
    // returning to a page
    setTimeout("backButtonOverrideBody()", 250);
}

//...................................................................................................//     

function showKeyCode(e) {
    //    var version = navigator.appVersion;
    //    var keycode =(window.event) ? event.keyCode : e.keyCode;
    //    if(keycode == 27)
    //    {
    //        if($get('divCalculator').style.display=='block')
    //        {
    //            $get('divCalculator').style.display='none';
    //            return false;
    //        }
    //    }
}

//...............................................................................................................

function RefreshPage() {
    var hdnTopBpInfo = top.$get('ctl00_cphPageContents_GVUC_hdnMasterBPIn');
    if (hdnTopBpInfo && hdnTopBpInfo.value.length > 0) {
        AjaxMethods.SetBPEINFO(hdnTopBpInfo.value);
    }
    var thisPage = top.window.location + ""; //Cast the object to string.
    Redirect(thisPage);
}

//...............................................................................................................

//Disable Enter key functionality.
function noenter(eve) {
    return !(eve && eve.keyCode == 13);
}

//..............................................................................................................................//


function jqAlert(txt, a_title, a_modal, onClose) {
    if (typeof (a_title) == 'undefined' || a_title == null) {
        a_title = 'LAjit says..';
    }
    if (typeof (a_modal) == 'undefined' || a_modal == null) {
        a_modal = false;
    }
    var div = $get('divJQAlert')
    if (div == null) {
        div = document.createElement('div');
        div.style.zIndex = 1000020099;
        div.id = 'divJQAlert';
        div.style.display = 'none';
        div.style.width = '300px';
        div.style.height = '100px';

        div.innerHTML = txt;
        document.body.appendChild(div);
        jQuery('#' + div.id).dialog({ title: a_title,
            autoOpen: true,
            bgiframe: false,
            modal: a_modal,
            //    			                         show:'clip',
            draggable: false,
            resizable: false,
            closeOnEscape: true,
            close: onClose,
            buttons: { "Ok": function () { jQuery(this).dialog("close"); } }

        });
    }
    else {
        div.innerHTML = txt;
        jQuery('#' + div.id).dialog('open');
    }
}

//...................................................................................................//
function ShowCalculator(objID) {
    var myCalc = jQuery('#' + objID);
    if (myCalc.attr('disabled') != true) {
        myCalc.calculator
        ({
            onClose: function (val, obj) { myCalc.calculator('destroy'); CalcClose(val, obj); },
            layout: ['MC_0_._=_+' + jQuery.calculator.CLOSE, 'MR_1_2_3_-' + jQuery.calculator.USE, 'MS_4_5_6_*' + jQuery.calculator.ERASE, 'M+_7_8_9_/']
        });
        var targetFocus = myCalc.parent('td').siblings('td').find('img')[0];
        targetFocus.focus();
        myCalc[0].focus();
    }
}

//The Calculator OnClose event.
function CalcClose(val, obj) {
    var objTarget = obj._input[0];
    var workStr = objTarget.id;
    var stInd = workStr.indexOf('_grdVwBranch');
    var CGVCClientID = workStr.substring(0, stInd);
    if (IsPaging(CGVCClientID)) {
        var rowIndex = workStr.split('_')[4].replace('GVRow', '');
        var hdnModId = CGVCClientID + '_hdnModRows';
        SetModRowIndices(hdnModId, rowIndex);
    }

    //Get the Amount Column Index from the target textbox.
    var amtColIndex = jQuery(objTarget).attr('isamount');
    //Update the Instantaneous sum.
    ShowInsantaneousSum(CGVCClientID + '_grdVwBranch', amtColIndex);
    FilterAmount(objTarget);
}

//Get all the visible controls in the Entry Form.
var g_objFormCntls;
function GetFrmElements() {
    if (g_objFormCntls == null) {
        g_objFormCntls = jQuery('tr[id^="ctl00_cphPageContents_tr"] select:visible, tr[id^="ctl00_cphPageContents_tr"] input:visible')
               .add('tr[id*="grdVwBranch_GVRow"] select:visible, tr[id*="grdVwBranch_GVRow"] input:visible');
    }
    return g_objFormCntls;
}

//Status - True to show the scrollbars, false otherwise.
function SetScrollBars(status) {
    //    var qspDepth = queryString("depth");
    //    if(qspDepth && qspDepth>0)
    //    {
    var parentIF = this.frameElement;
    if (parentIF) {
        if (status) {
            if (jQuery.browser.mozilla) {
                //                    parentIF.scrolling="yes";
                jQuery(parentIF).css('overflow', '');
            }
            else {
                SetScrollIE('yes');
            }
        }
        else {
            if (jQuery.browser.mozilla) {
                //parentIF.scrolling="no";
                jQuery(parentIF).css('overflow', 'hidden');
            }
            else {
                SetScrollIE('no');
            }
        }
    }
    //    }    
}

function SetScrollIE(status) {
    var eleRef = this.parent.frames['iframePage'];
    var bodyElementsArray = eleRef.document.getElementsByTagName('BODY');
    if (bodyElementsArray != null) {
        var bodyElement = bodyElementsArray[0];
        bodyElement.scroll = status;
    }
}
//...................................................................................................//

function AttachCollapsibleExt(pnlTitleId, pnlContId, imgId) {
    var currentTheme = $get('ctl00_hdnThemeName').value;
    $get(pnlTitleId).onclick = function () {
        jQuery('#' + pnlContId).slideToggle('slow');
        var imgIcon = document.getElementById(imgId).getAttribute('src');
        var imgIconSrc = imgIcon.split('/');
        if (imgIconSrc[4] == 'minus-icon.png') {
            jQuery('#' + imgId).attr('src', g_cdnImagesPath + 'Images/plus-icon.png');
        }
        else {
            jQuery('#' + imgId).attr('src', g_cdnImagesPath + 'Images/minus-icon.png');
        }
    };
}

//Attached the Calendar extender to be called later on a button click.
function AttachCal(elemId, fmt, isParent) {
    var calShowOn, bImage, bImageOnly, bText;
    if (isParent) {
        //        var currentTheme=$get('ctl00_hdnThemeName').value;
        calShowOn = 'none';
        //        bImage='../App_Themes/'+currentTheme+'/Images/calendar-icon.gif';
        //        bImageOnly=true;
        //        bText='Calendar';
    }

    var elem = jQuery('#' + elemId);
    elem.datepicker({ dateFormat: fmt,
        changeMonth: true,
        changeYear: true,
        onClose: CalClose,
        duration: 'fast',
        showAnim: 'blind',
        showOn: calShowOn,
        buttonImage: bImage,
        buttonImageOnly: bImageOnly,
        buttonText: bText
    });
    //Add the trigger-class to the image trigger so that it is not considered external click event.
    elem.parent().next('td').find('img[onclick]').addClass('ui-datepicker-trigger');
}

//Invokes the already-attached calendar.
function ShowCalendar(objID) {
    var myCal = _get(objID);
    if (jQuery(myCal).attr('disabled') != true) {
        var dpInst = jQuery.datepicker;
        if (dpInst._datepickerShowing && dpInst._lastInput == myCal)
            dpInst._hideDatepicker();
        else
            dpInst._showDatepicker(myCal);
    }
    return false;
}

//JQ DatePicker Onclose event.
function CalClose(txt, obj) {
    if (txt.length != 0) {
        //Set the focus to the next element
        var found = false;
        var tarId = obj.input[0].id;
        var arrEls = GetFrmElements(); //.filter(':not([id*=_CGVUC_])');
        arrEls.each(function (i) {
            if (found) {
                if (this.type == 'checkbox' && this.id.indexOf('_chkBxSelectRow') != -1) return;
                this.focus();
                return false; //Break the each loop.
            }
            else if (this.id == tarId) {
                found = true;
            }
        });
    }
}

//.........................................................................................................................................//

//A compacted function call for document.getElementById()
function _get(id) {
    return document.getElementById(id);
}
