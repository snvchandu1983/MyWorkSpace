
//Validates all the processes associated with the BProcessLinks.
function ValidateBPGIDRow() {
    try {
        var tblClientID = 'ctl00_cphPageContents_pnlBPCContainer';
        var objProcLinks = document.getElementById(tblClientID);
        if (objProcLinks == null) {
            return;
        }
        objProcLinks = GetChild(objProcLinks);
        if (objProcLinks == null) {
            return;
        }
        for (var i = 0; i < objProcLinks.rows.length; i++) {
            var row = objProcLinks.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                var objCell = row.cells[j].firstChild.firstChild;
                if (objCell == null) { // The cell consists of the separator.
                    continue;
                }
                var objHRef = objCell.getAttribute("href").split(',');
                if (objHRef.length <= 1) {
                    return;
                }
                var processBPGID = objHRef[0].replace(/'/g, "");
                var redirectPage = objHRef[1].replace(/'/g, "");
                var bpeInfoClientID = objHRef[2].replace(/'/g, "");
                var processName = objHRef[3].replace(/'/g, "");

                //Get the current BPINFO.
                var bpInfo;
                var hdnGVBPEINFO = document.getElementById(bpeInfoClientID);
                if (hdnGVBPEINFO && trim(hdnGVBPEINFO.value).length > 0) {
                    bpInfo = hdnGVBPEINFO.value;
                }
                else {
                    hdnGVBPEINFO = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnGVBPEINFO');
                    if (hdnGVBPEINFO) {
                        bpInfo = hdnGVBPEINFO.value;
                    }
                }

                if (processName.indexOf("BPGID") != -1) {
                    //Get the Row-level BPGID and PageInfo information.
                    var xDocCurrentRow = loadXMLString("<Root>" + bpInfo + "</Root>");
                    var nodeRow = xDocCurrentRow.getElementsByTagName("Rows")[0];
                    var rowBPGID = nodeRow.getAttribute(processName); //Row-Level BPGID
                    var pageInfoAttName = "PageInfo";
                    var BPBIDIndex = parseInt(trim(processName.replace("BPGID", ""))); //Ex:Get the 2 from BPGID2
                    if (!isNaN(BPBIDIndex)) { //If an index is present
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
                        //Hide the current cell along with the separator
                        row.cells[j].style.display = "none";
                        if (row.cells[j + 1]) {
                            row.cells[j + 1].style.display = "none";
                        }
                    }
                    else {
                        //Show em.
                        row.cells[j].style.display = "";
                        if (row.cells[j + 1]) {
                            row.cells[j + 1].style.display = "";
                        }
                    }
                }
            }
        }
    }
    catch (error) {
        if (g_IsDebugging) {
            alert(error.message);
        }
    }
}

//...................................................................................................//

function GetChild(parentNode) {
    x = parentNode.firstChild;
    while (x && x.nodeType != 1) {
        x = x.nextSibling;
    }
    return x;
}

//...................................................................................................//
function SentEmail(objPages, objGridClientID, objRadID) {
    var BPINFO = document.getElementById(objGridClientID).value;
    var EmailIDS;
    var arrElements = document.getElementsByName('ctl00$cphPageContents$txtEmailing');
    var isText = false;
    for (var i = 0; i < arrElements.length; i++) {
        if (arrElements[i].value != '') {
            EmailIDS = arrElements[i].value;
            if (EmailIDS != null) {
                isText = true;
                break;
            }
            else {
                isText = false;
            }
        }
    }
    if (isText == true) {
        if (g_arrCols == null) {
            g_arrCols = new Array();
        }
        AjaxMethods.GridEmailPDF(BPINFO, objPages, EmailIDS, g_arrCols, callback_EmailStatus);
        return false;
    }
    else {
        //   debugger; 
        alert('Please enter e-mail ids');
        var arrRadElements = objRadID.id.replace(/_/g, "$");
        var radElements = document.getElementsByName(arrRadElements);
        for (var i = 0; i < radElements.length; i++) {
            if (radElements[i].checked == true) {
                radElements[i].checked = false;
                break;
            }
        }
    }
}
//...................................................................................................//
function callback_EmailStatus(response) {
    try {
        var StatusMessage = response.value;
        if (StatusMessage == null) {
            alert("Email Sent Successfully");
        }
        else {
            alert(StatusMessage);
        }
    }
    catch (error) {
        if (g_IsDebugging) {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : callback_EmailStatus()\nFileName : GridViewUserControl.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}
//...................................................................................................//

var pp;
function PrintPage(pages, gridClientID) {
    pp = window.open('', 'NewWindow', 'status=0,scrollbars=1,height=300,resizable=yes,location=no,menubar=no,toolbar=no');
    pp.document.writeln('Please wait..');
    var BPINFO = $get(gridClientID + '_hdnSearchBPInfo') || $get('ctl00_cphPageContents_hdnBPInfo');
    if (typeof (g_arrCols) == 'undefined' || g_arrCols == null) {
        g_arrCols = new Array();
    }
    AjaxMethods.GeneratePrintHTML(BPINFO.value, pages, callback_GenPrintHTML);
    return false;
}

//...................................................................................................//

function callback_GenPrintHTML(response) {
    try {
        if (response.error != null) {
            alert(response.error);
            return;
        }
        var outHTML = response.value;
        if (outHTML != "") {
            pp.document.close();
            pp.document.open();
            pp.document.writeln(outHTML);
            pp.focus();
            pp.location.reload(false);
            pp.document.close();
            pp.print();
            pp.self.close();
        }
        else {
            pp.document.close();
            pp.document.open();
            pp.document.writeln("<center><b>");
            pp.document.writeln("No Rows to Print");
            pp.document.writeln("</b></center>");
            pp.focus();
            pp.location.reload(false);
            pp.document.close();
        }
    }
    catch (error) {
        if (g_IsDebugging) {
            g_ErrorText = "";
            g_ErrorText += "An error has occurred in this page";
            g_ErrorText += "\nFunction Name : callback_GenPrintHTML()\nFileName : GridViewUserControl.js";
            g_ErrorText += "\nError Description : " + error.message;
            alert(g_ErrorText);
        }
    }
}

//...................................................................................................//

function ShowPrintViewer(redirectPage, gridClientID) {
    //Get the type of report being requested from the query string of the url.
    var loc = redirectPage.split('?')[1];
    var param_value = false;
    var params = loc.split("&");
    for (i = 0; i < params.length; i++) {
        param_name = params[i].substring(0, params[i].indexOf('='));
        if (param_name == 'RptType') {
            param_value = params[i].substring(params[i].indexOf('=') + 1)
        }
    }
    //Append the GroupBy Column selection if any.
    if (param_value) {
        var grpByCol = jQuery('.qtip-content #' + gridClientID + '_ddl' + param_value + 'GrpBy ').val();
        if (typeof (grpByCol) != "undefined" && grpByCol != "-1") {
            redirectPage += '&GRP=' + grpByCol;
        }
    }

    var qspPopUp = queryString("PopUp");
    if (qspPopUp) {
        redirectPage = redirectPage + "&PopUp=PopUp";
    }
    ClearSessionExpireTimeOuts();
    redirectPage = "../" + redirectPage;
    //Whether to display in an IFrame or in a new browser tab.
    var isPopUp = document.getElementById(gridClientID + '_hdnPreviewInPopup').value;
    if (isPopUp == "1") {
        DisplayPopUp(true);
        var targetIFrame = document.getElementById("iframePage");
        targetIFrame.src = redirectPage;
    }
    else {
        //Set the session to store the Print Columns selected using the column chooser.
        if (typeof (g_arrCols) !== 'undefined') {
            AjaxMethods.SetPrintColumns(g_arrCols);
        }
        OpenNewWindow(redirectPage, 'PrintViewer')
    }
    return false;
}

//...................................................................................................//

function OpenNewWindow(url, name) {
    newwindow = window.open(url, name);
    if (window.focus) { newwindow.focus() }
    return false;
}

//...................................................................................................//




function ShowChartViewer(redirectPage, gridClientID, xAxisValName, yAxisValName) {
    //This part used exclusive chart area
    var IFrameWidth = 932; //Values synonomous to those of BtnsUC IFramePage
    var IFrameHeight = 525;

    //Qtip issue fixed on 290310 
    var xAxisVal;
    var yAxisVal;
    var arrxElements = document.getElementsByName(xAxisValName);
    if (arrxElements[1] != null) {
        xAxisVal = arrxElements[1].value;
    }

    var arryElements = document.getElementsByName(yAxisValName);
    if (arryElements[1] != null) {
        yAxisVal = arryElements[1].value;
    }
    redirectPage = "../" + redirectPage;
    //    //Whether to display in an IFrame or in a new browser tab.
    //    var isPopUp=$get(gridClientID+'_hdnPreviewInPopup')?$get(gridClientID+'_hdnPreviewInPopup').value:"1";
    //    
    //    if(isPopUp == "1")
    //    {
    var targetIFrame = document.getElementById("iframePage");

    var qspDepth = queryString("depth");
    if (qspDepth) {//If already exists increment and call the new Popup frame.
        targetIFrame.width = IFrameWidth - (qspDepth * g_IFDec.Width);
        targetIFrame.height = IFrameHeight - (qspDepth * g_IFDec.Height);
        qspDepth++;
    }
    else {//Calling a popup for the first time.
        qspDepth = 1;
    }

    DisplayPopUp(true);
    redirectPage = "./" + redirectPage + "?PopUp=PopUp&depth=" + qspDepth + "&x=" + xAxisVal + "&y=" + yAxisVal;
    //Calling the C# method to set the Session 'LinkBPinfo'.
    //CommonUI.SetBPINFOSession(BPInfo);
    targetIFrame.src = redirectPage;
    //To show loading image
    ShowIframeProgress();
    //    }
    //    else
    //    {
    //        //CommonUI.SetBPINFOSession(BPInfo);
    //        //Posting the page       
    //        //document.getElementById('hdnBPInfo').value =  BPInfo; 
    //        //To redirect and show like normal page.
    //        window.open(redirectPage, "ChartViewer");
    //        //Redirect(redirectPage);

    //    }
    return false;
}


//...................................................................................................//


function ShowSearch(gridID) {
    //Hide the Search Image Button
    var findId = gridID + '_ctl01_imgBtnQuickSearch';
    var imgBtnSearch = document.getElementById(findId);
    var hdnCSFindVisible = document.getElementById("ctl00_cphPageContents_GVUC_hdnCSFindVisible");
    if (hdnCSFindVisible == null) {
        hdnCSFindVisible = document.getElementById("ucGridView_hdnCSFindVisible");
    }
    var imgExpandFind = document.getElementById(gridID + '_ctl01_imgBtnExpandFind');
    var currentTheme = document.getElementById('ctl00_hdnThemeName').value;
    //if(imgBtnSearch.style.display=='')
    if (jQuery.FindVisible) {
        imgBtnSearch.style.display = 'none';
        hdnCSFindVisible.value = '0';
        imgExpandFind.src = g_cdnImagesPath + "Images/add_symbol.png";
        //alert(jQuery('#'+gridID+' tr:first').height());//90
    }
    else {
        imgBtnSearch.style.display = '';
        hdnCSFindVisible.value = '1';
        imgExpandFind.src = g_cdnImagesPath + "Images/minus-icon.png";
        //alert(jQuery('#'+gridID+' tr:first').height());//34
    }

    var objGrid = document.getElementById(gridID);
    for (var cellCount = 0; cellCount <= objGrid.rows[0].cells.length; cellCount++) {
        var clientIDTR = gridID + "_ctl01_trFind" + cellCount;
        var objTRFind = document.getElementById(clientIDTR);
        if (objTRFind) {
            if (jQuery.FindVisible) {
                //objTRFind.style.display='none';
                jQuery(objTRFind).fadeOut(100);
            }
            else {
                //objTRFind.style.display='';
                jQuery(objTRFind).fadeIn(500);
            }
            //jQuery(objTRFind).slideToggle(400);
        }
    }


    if (typeof (jQuery.FindVisible) === 'undefined') {
        jQuery.FindVisible = false;
    }
    jQuery.FindVisible = !jQuery.FindVisible;
    //jQuery('#'+gridID+' tr:first').height(90);
}

//...................................................................................................//

function BuildQuickSearchRow(masterID) {
    var strQuickSearch = "";
    var gridID = masterID + '_grdVwContents';
    var objGrid = document.getElementById(gridID);

    for (var cellCount = 0; cellCount < objGrid.rows[0].cells.length; cellCount++) {
        var clientIDTxt = gridID + "_ctl01_txtFind" + cellCount;

        var objTxtFind = document.getElementById(clientIDTxt);
        //alert(clientIDTxt+"\n"+objTxtFind);
        if (objTxtFind) {
            strQuickSearch += objTxtFind.value + "~::~";
        }
    }
    document.getElementById(masterID + '_hdnFindCriteria').value = strQuickSearch;
    //alert(strQuickSearch);
}

//...................................................................................................//

// Gets the width of the auto grow td of the Main GV panel and sets in the variable g_HeaderWidth.
function GetTitleElementWidth() {
    var pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_htcCPGV1Auto');
    if (pnlHeaderTitle == null) {
        pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_htcEntryFormAuto'); //The title displayed in page's Add Load mode
    }

    if (pnlHeaderTitle) {
        g_HeaderWidth = pnlHeaderTitle.width;
    }
}

//...................................................................................................//

// Sets the width of the auto grow td of the Main GV panel to the value found in the variable g_HeaderWidth.
function SetTitleElementWidth() {
    //Maintain the same width as found in g_HeaderWidth variable
    var pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_htcCPGV1Auto');
    if (pnlHeaderTitle == null) {
        pnlHeaderTitle = document.getElementById('ctl00_cphPageContents_htcEntryFormAuto'); //The title displayed in page's Add Load mode
    }
    if (pnlHeaderTitle) {
        if (g_HeaderWidth != "0") {
            pnlHeaderTitle.width = g_HeaderWidth;
        }

    }
}

//...................................................................................................//

function HideProgress() {
    var divLayer = document.getElementById('layer');
    var divImage = document.getElementById('box');
    if (divLayer) {
        document.body.removeChild(divLayer);
    }
    if (divImage) {
        document.body.removeChild(divImage);
    }
}

//...................................................................................................//

//Displays a update progress on the screen
function ShowProgress(isJqGrid) {
    var width = document.documentElement.clientWidth + document.documentElement.scrollLeft;
    //Add a div in the middle of the page on top of the semi-transparent layer.
    if (document.getElementById('box') == null)//Check whether postback has completed within the ShowProgress Delay.
    {
        if (prm.get_isInAsyncPostBack() || isJqGrid) {
            var currentTheme = document.getElementById('ctl00_hdnThemeName').value;
            var div = document.createElement('div');
            div.style.zIndex = 100002009;
            div.id = 'box';
            div.style.position = (navigator.userAgent.indexOf('MSIE 6') > -1) ? 'absolute' : 'fixed';
            div.style.top = '200px';
            div.style.left = (width / 2) - 50 + 'px';
            div.innerHTML = '<img alt="Please wait.." src="' + g_cdnImagesPath + 'Images/ajax-loader.gif" />';
            document.body.appendChild(div);
        }
    }
}

//...................................................................................................//

var g_PostBackElement;
var g_DoEndRequestHandling;
function BeginRequestHandler(sender, args) {

    g_PostBackElement = args.get_postBackElement();
    if (typeof ResetBlinkObjects == 'function') {
        ResetBlinkObjects();
    }
    jQuery('.qtip.qtip-grid.qtip-active').remove();

    //alert(g_PostBackElement.id);
    //Show the update progress bar and set the time out.
    //if(g_PostBackElement.id.indexOf('lnkBtn') == -1 && g_PostBackElement.id.indexOf('FV')== -1)//Don't show for Master Page Full View Events
    {
        setTimeout('ShowProgress()', 0);
    }
    g_DoEndRequestHandling = true;
}

//...................................................................................................//

function EndRequestHandler(sender, args) {
    if (g_DoEndRequestHandling) {
        //setTimeout('SetTitleElementWidth()',0);//To maintain the title element's width constant across async postbacks
        setTimeout('SetChildHeaderWidth()', 0);
        setTimeout('SetParentGridLinks()', 0);
        setTimeout('HideProgress()', 0);

        //Revert the variable to true if it has been changed to false earlier
        if (typeof (g_temp) != "undefined" && g_temp == false) {
            g_temp = true;
        }

        if (g_PostBackElement.id.indexOf('imgBtnTTnNavigate') != -1) {
            var cpeMainGrid = $find('BIDMainGrid');
            if (cpeMainGrid) {
                if (!cpeMainGrid._checkCollapseHide()) {
                    cpeMainGrid.collapsePanel();
                }
            }
        }

        if (g_PostBackElement.id == 'ctl00_cphPageContents_CGVUC_lnkBtnAddRows') {
            var grid = jQuery("#ctl00_cphPageContents_CGVUC_pnlGVBranch");
            var scrollPos = grid.attr('scrollHeight');
            grid.animate({ scrollTop: '+=' + scrollPos }, 1000);
        }

        g_DoEndRequestHandling = false;
    }
}

//...................................................................................................//

function SetExitConfirmationToFalse() {
    if (g_temp) {
        g_temp = false;
    }
}

//...................................................................................................//

//Sets the widths of the child object headers if any to that of the main header's.
function SetChildHeaderWidth() {
    var childElement = document.getElementById('ctl00_cphPageContents_pnlGVBranch');
    var currentPage = window.location + ""; //Cast the object to string.
    if (currentPage.toUpperCase().indexOf('COMMERCIAL.ASPX') != -1) {
        return;
    }
    if (childElement) {
        var headerElement = document.getElementById('ctl00_cphPageContents_pnlCPGV1Title');
        if (headerElement == null) {
            headerElement = document.getElementById('ctl00_cphPageContents_pnlEntryFormTitle');
        }
        if (headerElement) {
            var elementBounds = Sys.UI.DomElement.getBounds(headerElement);
            var adjustedWidth = elementBounds.width - 4;
            childElement.style.width = adjustedWidth + "px";
        }
    }
}


function InitToolTips() {
    var gridStr = 'ctl00_cphPageContents_GVUC';
    if ($get('ctl00_cphPageContents_GVUC_updtPnlGrdVw') == null) {
        gridStr = 'ctl00_cphPageContents_PCGVUC';
        if ($get('ctl00_cphPageContents_PCGVUC_updtPnlGrdVw') == null) {
            gridStr = 'ucGridView'; //Detailed View
        }
    }

    var hdnToolTipJS = $get(gridStr + '_hdnToolTipJS');
    //Initialise the Row-level tooltip.
    if (hdnToolTipJS && hdnToolTipJS.value.length > 0) {
        var arrFunctions = hdnToolTipJS.value.split(':');
        for (var cntr = 0; cntr < arrFunctions.length; cntr++) {
            var index = arrFunctions[cntr];
            if (index.length > 0) {
                if (index <= 9) {
                    index = "0" + index;
                }
                var rowInfoClientID = gridStr + "_grdVwContents_ctl" + index + "_hdnRowInfo"; //ctl00_cphPageContents_GVUC_grdVwContents_ctl02_hdnRowInfo
                var objTargetId = gridStr + "_grdVwContents_ctl" + index + "_imgBtnTTnNavigate"; //ctl00_cphPageContents_GVUC_grdVwContents_ctl02_imgBtnTTnNavigate
                ShowToolTipPopup(rowInfoClientID, objTargetId);
            }
        }
    }
    var arrHvrMns = new Array('divHvrDirectPrint', 'divHoverPageSelectChart', 'divHoverPageSelectPDF', 'divHoverPageSelectExcel', 'divHoverPageSelectHTML', 'divHoverPageSelectEmailPDF', 'divHvrEmailIDs', 'divHoverPageSelectMSWord')
    var arrCtrlIds = new Array();
    arrCtrlIds.push(gridStr + '_imgBtnPrint');
    arrCtrlIds.push(gridStr + '_imgBtnChart');
    arrCtrlIds.push(gridStr + '_imgBtnPDF');
    arrCtrlIds.push(gridStr + '_imgBtnExcel');
    arrCtrlIds.push(gridStr + '_imgBtnHtml');
    arrCtrlIds.push(gridStr + '_imgBtnEmailPDF');
    arrCtrlIds.push('aProcessMenu');
    arrCtrlIds.push(gridStr + '_imgBtnMSWord');
    for (var index = 0; index < 8; index++) {
        var objTarget = $get(arrCtrlIds[index]);
        if (objTarget != null) {
            var ttContent;
            var ttHide;
            var ttPosition = { corner: { target: 'topLeft', tooltip: 'bottomRight' }, adjust: { screen: true} };
            if (arrCtrlIds[index] == 'aProcessMenu') {
                ttContent = { url: '../HTML/GenRptHover.htm', title: { text: '<span style="padding:2px">Print/EMail</span>'} };
                ttPosition.target = 'mouse';
                ttPosition.adjust.mouse = false;
                ttPosition.corner.tooltip = 'topLeft';
                ttHide = { when: { event: 'unfocus'} };
            }
            else {
                ttContent = { text: $get(arrHvrMns[index]).innerHTML, title: { text: ClipPrintType(arrHvrMns[index])} };
                ttHide = { delay: 100, fixed: true };
            }

            var objQTip = jQuery(objTarget).qtip(
            {
                content: ttContent,
                position: ttPosition,
                hide: ttHide,
                //style: { tip:'bottomRight',padding: '5px 5px',name: 'grid', border: { width: 2, radius: 6 } },
                style: { tip: 'bottomRight', padding: '5px 5px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A'} },
                show: { effect: { type: 'fade', length: 300} }
            });
        }
    }

    //Init the clear search context menu.
    jQuery('#' + gridStr + '_grdVwContents_ctl01_imgBtnQuickSearch').contextMenu('clearMenu', {
        bindings: { 'open': function () { ClearSearchFields(gridStr); } }
    });
}

function ClipPrintType(str) {
    var i = str.indexOf('PageSelect');
    if (str == 'divHoverPageSelectEmailPDF') {
        return 'E-Mail Report';
    }
    if (i !== -1) {
        var j = str.length;
        i = i + 10;
        return 'Print ' + str.substring(i, j);
    }
    return 'Print';
}

function ShowToolTipPopup(rowInfoClientID, objTargetId) {
    //Tip(GenerateToolTipTable(document.getElementById(rowInfoClientID).value), COPYCONTENT, false, BORDERWIDTH, 1, PADDING, 0);
    //console.log("ToolTip initialised for : "+rowInfoClientID +"\t"+objTargetId);
    var objTarget = $get(objTargetId);
    if (objTarget) {
        var objQTip = jQuery(objTarget).qtip(
          {
              content: {
                  text: GenerateToolTipTable(document.getElementById(rowInfoClientID).value),
                  title: { text: 'About me' }
              },
              position: { corner: { target: 'bottomRight', tooltip: 'topLeft' }, adjust: { screen: true} },
              hide: {
                  fixed: false //Make it fixed so it can be hovered over
                  //when: { event: 'click',event: 'unfocus', event:'mouseout' }
              },
              style: {
                  //tip:'topLeft',padding: '5px 5px',name: 'grid', border: { width: 2, radius: 6 }
                  tip: 'topLeft', padding: '5px 5px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A' }
              },
              show: { effect: { type: 'fade'} }
          });
        objQTip.bind("click", function (e) { objQTip.qtip("hide"); objQTip.qtip("destroy"); });
    }
}

//...................................................................................................//

function GenerateToolTipTable(xmlText) {
    var xmlDoc = loadXMLString(xmlText);
    var nodeRows = xmlDoc.getElementsByTagName("Rows")[0];
    var ttColIndices = nodeRows.getAttribute("ToolTipInfo").split(',');
    var tableInnerText = "<table cellpadding='2' cellspacing='0' >";
    for (var index = 0; index < ttColIndices.length; index++) {
        var strNodeName = nodeRows.attributes[ttColIndices[index]].nodeName + "";
        var strNodeValue = nodeRows.attributes[ttColIndices[index]].nodeValue + "";
        if (strNodeValue == "") {
            continue;
        }
        if (strNodeName.toUpperCase() == 'TOOLTIP') {
            //Don't show ToolTip attribute name
            tableInnerText += "<tr><td colspan='2'>" + strNodeValue.replace(/~/g, "<br>") + "</td></tr>";
        }
        else {
            tableInnerText += "<tr><td>" + strNodeName
                             + "</td><td>" + strNodeValue.replace(/~/g, "<br>") + "</td></tr>";
        }
    }
    tableInnerText += "</table";
    return tableInnerText;
}

//...................................................................................................//

function OnColumnLinkClick(BPGID, redirectPage, rowInfoClientID, TrxID, TrxType, formInfoClientID, linkText, isPopUp, processName) {
    //alert(BPGID + "\n" + redirectPage + "\n" +rowInfoClientID+ "\n" +TrxID+ "\n" +TrxType+ "\n" +formInfoClientID+ "\n" +linkText+ "\n" +isPopUp+ "\n" +processName);
    linkText = linkText.replace(/~::~/g, " "); //Restore the spaces.
    linkText = escapeHTML(linkText);

    //added from jqgrid
    var rowXML;
    if (document.getElementById(rowInfoClientID)) {
        rowXML = document.getElementById(rowInfoClientID).value;
    }
    else {
        rowXML = rowInfoClientID;
    }
    var formInfoBPGID;
    var formInfoPage;
    if (formInfoClientID.indexOf('~::~') != -1) {
        formInfoBPGID = formInfoClientID.split('~::~')[0];
        formInfoPage = formInfoClientID.split('~::~')[1];
    }
    else {
        var dbFrmInfo = document.getElementById(formInfoClientID).getAttribute("FormInfo");
        if (dbFrmInfo) {
            //In DashBoard FormInfo is assigned to the table containing the gridview rather than storing it an hidden variable.
            formInfoBPGID = dbFrmInfo.split('~::~')[0];
            formInfoPage = dbFrmInfo.split('~::~')[1];
        }
        else {
            var frmInfo = document.getElementById(formInfoClientID);
            formInfoBPGID = frmInfo.value.split('~::~')[0];
            formInfoPage = frmInfo.value.split('~::~')[1];
        }
    }

    //------------
    var callingObjXML = "<CallingObject><BPGID>" + formInfoBPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + formInfoPage + "</PageInfo><Caption>" + linkText + "</Caption></CallingObject>";
    var hdnBPEINFO = rowXML + callingObjXML;
    if (processName.indexOf("BPGID") != -1) {
        //Get the Row-level BPGID and PageInfo information.
        var xDocCurrentRow = loadXMLString("<Root>" + rowXML + "</Root>");
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
            BPGID = rowBPGID;
        }
        if (!IsURLValid(redirectPage) || (BPGID == null || trim(BPGID) == "")) {
            alert("No Business Process is associated with this record!!");
            return;
        }
    }
    //NoPageProcess1
    if (processName.indexOf("NoPageProcess") != -1) {
        var ProcessStatus = AjaxMethods.SubmitProcess(BPGID, hdnBPEINFO);
        var ProcessResult = ProcessStatus.value.split("~");
        if (ProcessResult.length == 5) {
            BPGID = ProcessResult[0];
            redirectPage = ProcessResult[1];
            processName = ProcessResult[2];
            isPopUp = ProcessResult[3];
            var rowXML = ProcessResult[4];
            document.getElementById(rowInfoClientID).value = rowXML;
            OnColumnLinkClick(BPGID, redirectPage, rowInfoClientID, TrxID, TrxType, formInfoClientID, linkText, isPopUp, processName)
            return;
        }
        else if (redirectPage == "MsgLine" || redirectPage == "MsgBox") //In case of gridview always display an alert as pnlEntryForm may not be available at all times.  
        {
            if (document.getElementById("ctl00_cphPageContents_lblmsg")) {
                document.getElementById("ctl00_cphPageContents_lblmsg").innerHTML = "";
            }
            alert(ProcessStatus.value);
        }
        return;
    }

    //added from jqgrid
    //redirectPage="Common/Default.aspx";//JQGrid overrride.
    //redirectPage="Common/GenView.aspx";//JQGrid overrride. 
    redirectPage = GetRedirectPage(redirectPage); //Function found in Common.js
    if (isPopUp == "1") {
        var targetIFrame = document.getElementById("iframePage");
        var qspDepth = queryString("depth");
        //        if(qspDepth){//If already exists increment and call the new Popup frame.
        //            targetIFrame.width = g_IFrameWidth-(qspDepth*g_IFDec.Width);
        //            targetIFrame.height = g_IFrameHeight-(qspDepth*g_IFDec.Height);
        //            qspDepth++;
        //        }
        //        else{//Calling a popup for the first time.
        //            targetIFrame.width = g_IFrameWidth;
        //            targetIFrame.height = g_IFrameHeight;
        //            qspDepth=1;
        //        }
        qspDepth = 1;
        redirectPage = "../" + redirectPage + "?PopUp=PopUp&depth=" + qspDepth;
        //Calling the C# method to set the Session 'LinkBPinfo'.
        CommonUI.SetLinkPopUpSession(BPGID, hdnBPEINFO);
        var iframeId = ShowIFrame(redirectPage);
        SaveToHistory(BPGID, hdnBPEINFO, redirectPage, iframeId);
        //        //Remove the scrollbars of the parent IFrame.
        //        SetScrollBars(false);
        //        DisplayPopUp(true);
        //        ShowIframeProgress();
        //        targetIFrame.src = redirectPage;
    }
    else {
        UpdateNavHistoryXML(rowXML + callingObjXML);
        //Query String Parameter Depth : Indicates the number of Iframes open at any given time.
        var isPopUp = queryString("PopUp");
        if (isPopUp || this.frameElement) {
            //Case where depth exists and IsPopUp=0. 
            //This implies use the current IFrame window and post the page to that window.
            redirectPage = "../" + redirectPage + "?PopUp=PopUp&depth=" + qspDepth;
            CommonUI.SetLinkPopUpSession(BPGID, hdnBPEINFO);
            //           parent.document.getElementById("iframePage").src = redirectPage;
            var frameId = RedirectIFrame(redirectPage);
            SaveToHistory(BPGID, hdnBPEINFO, redirectPage, frameId);
        }
        else {
            redirectPage = "../" + redirectPage;
            var bpInfoRedirect = "<bpinfo><BPGID>" + BPGID + "</BPGID>" + rowXML + callingObjXML + "</bpinfo>";
            document.getElementById('hdnBPInfo').value = bpInfoRedirect;
            Redirect(redirectPage);
            SaveToHistory(BPGID, bpInfoRedirect, redirectPage);
        }
    }
}

//...................................................................................................//

//This function is used to achieve Next & Previous grid records functionality. Called in CommonUI.cs
function NavigateRows(direction, postBackString) {
    var hdnSelectedRowIndex = document.getElementById('ctl00_cphPageContents_GVUC_hdnSelectedRowIndex');
    var selectedRowIndex = parseInt(hdnSelectedRowIndex.value) + 2; //02 difference between the rows in gridview.  
    if (direction == "F") {
        selectedRowIndex++;
    }
    else {
        selectedRowIndex--;
    }
    var postBackSplit = postBackString.split('$');
    var ctlIndex = postBackSplit[4];
    if (selectedRowIndex >= 10) {
        postBackSplit[4] = 'ctl' + selectedRowIndex;
    }
    else {
        postBackSplit[4] = 'ctl0' + selectedRowIndex;
    }
    var postBackString = postBackSplit.join('$');

    var id = 'ctl00_cphPageContents_GVUC_grdVwContents_' + postBackSplit[4] + '_imgBtnTTnNavigate';
    var imgBtnTTnNavigate = document.getElementById(id);
    //if pagesize is 10, after 10 Next botton should not be clickable. so checking with button existanace.
    //similary selectedRowIndex=1 means no record before that. so no previous button functionality.
    if (imgBtnTTnNavigate != null && selectedRowIndex != 1) {
        window.setTimeout(postBackString, 0);
    }
}

//...................................................................................................//
function ClearSearchFields(gridstr) {
    var sender = $get(gridstr + '_grdVwContents_ctl01_imgBtnQuickSearch');
    gridstr = gridstr + '_grdVwContents';
    var objGrid = document.getElementById(gridstr);
    for (var cellCount = 0; cellCount <= objGrid.rows[0].cells.length; cellCount++) {
        var clientIDTR = gridstr + "_ctl01_trFind" + cellCount;
        var objTRFind = document.getElementById(clientIDTR);
        if (objTRFind) {
            var objTxt = jQuery(objTRFind).find('input:text')[0];
            objTxt.focus();
            objTxt.value = '';
        }
    }
    sender.focus();
}

function InitToolTipPopup(nodeRow, objTargetId, arrTTNames) {
    //Tip(GenerateToolTipTable(document.getElementById(rowInfoClientID).value), COPYCONTENT, false, BORDERWIDTH, 1, PADDING, 0);
    var objTarget = $get(objTargetId);
    if (objTarget) {
        var objQTip = jQuery(objTarget).qtip(
          {
              content: {
                  text: GenerateToolTip(nodeRow, arrTTNames),
                  title: { text: 'About me' }
              },
              position: { corner: { target: 'bottomRight', tooltip: 'topLeft'} },
              hide: {
                  fixed: false// Make it fixed so it can be hovered over
              },
              style: {
                  tip: 'topLeft', padding: '5px 5px', name: 'grid', border: { width: 2, radius: 6 }
              },
              show: { effect: { type: 'fade'} },
              api: { onHide: function (w) { objTarget.parentNode.parentNode.style.backgroundColor = ''; } }
          });
        objQTip.bind("click", function (e) { objQTip.qtip("hide"); });
    }
}

//ToolTip for jqGrid
function GenerateToolTip(nodeRow, arrTTNames) {
    var tableInnerText = "<table cellpadding='2' cellspacing='0' >";
    for (var index = 0; index < arrTTNames.length; index++) {
        var strNodeName = arrTTNames[index];
        var strNodeValue = GetInnerText(nodeRow.getElementsByTagName(arrTTNames[index])[0]); //GetInnerText can be found in JQMethods.js
        if (strNodeValue == "") {
            continue;
        }
        if (strNodeName.toUpperCase() == 'TOOLTIP') {
            //Don't show ToolTip attribute name
            tableInnerText += "<tr><td colspan='2'>" + strNodeValue.replace(/~/g, "<br>") + "</td></tr>";
        }
        else {
            tableInnerText += "<tr><td valign='top'>" + strNodeName
                             + "</td><td>" + strNodeValue.replace(/~/g, "<br>") + "</td></tr>";
        }
    }
    tableInnerText += "</table";
    return tableInnerText;
}
 

