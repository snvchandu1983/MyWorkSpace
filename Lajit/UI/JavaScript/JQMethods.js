var gridTableId = "#tblGrid"; //With the hash-jQuery friendly.//Test from TFS 6/22/2010 4:06PM
var divPagerName = "#divPager";
var editButtonId = "#btnEdit";
var addButtonId = "#btnAdd";
var searchButtonId = "#btnAdvancedSearch";
var deleteButtonId = "#btnDelete";
var cloneButtonId = "#btnAddClone";
var detailsButtonId = "#btnAddDetails";
var findButtonId = "#btnSearch";
var attachButtonId = "#btnAttachment";
var noteButtonId = "#btnNote";
var secureButtonId = "#btnSecure";
var cancelButtonId = "#btnCancel";
var g_SelectIndex = '1';
//Stores the sorted column order after being updated by the column chooser.
var g_arrCols;
//Stores the id's of the Rows which have been modified(Applicable for InEditable grids).
var g_arrILEModRows;
var FileInfo;
var xDocOut;
var g_IsFirstLoad = true; //The state of the postback.
var g_SubGrid; //Stores information related to SubGrid.

function GridLoadComplete(xhr) {
    var objGrid = jQuery(gridTableId);
    if (xhr) {
        var isAutoLoadUponScroll = objGrid.getGridParam('scroll');
        if (isAutoLoadUponScroll && xDocOut) {
            var xDocCurrent = xhr.responseXML;
            var arrRows = xDocCurrent.getElementsByTagName('Rows');
            for (var i = 0; i < arrRows.length; i++) {
                var rowNode = arrRows[i].cloneNode(true);
                xDocOut.documentElement.appendChild(rowNode); //Copy the rows into the local repository.
            }
        }
        else { xDocOut = xhr.responseXML; }
    }
    InitSubGrid(objGrid);
    StyleSortColumns();
    CheckFooterRow();
    InitHeader(objGrid);
    DoFormLoad(objGrid, xDocOut);
    if (typeof (g_ShowSearch) !== 'undefined' && g_ShowSearch === true) {
        ShowSearchToolBar(objGrid, false);
    }


    //If a subgrid  is present put a tooltip indicating the split feature
    jQuery('.sgcollapsed', objGrid).attr('title', 'Split Transaction');
    HideProgress(); //Hide the progress if previously called from somewhere.

    g_IsFirstLoad = false;
}

function InitSubGrid(objGrid) {
    var isSubGridPresent = objGrid.getGridParam('subGrid');
    if (isSubGridPresent === true) {
        g_SubGrid = {}; //Just create the object and leave it. The current selected parent row index will be intialised in the OnSubGrid expand event.
    }
}

//Changes the style of the jqGrid Title to the LAjit-styled grid header.
function InitHeader(objGrid) {
    if (g_IsFirstLoad === false) {
        return;
    }
    if (window.location.href.toUpperCase().indexOf("GENVIEW.ASPX") != -1) {
        var divHeader = jQuery('.ui-jqgrid-titlebar').remove().css('padding', '0').css('border', '0');
        jQuery('#ctl00_cphPageContents_jqGrid').prepend(divHeader).css('width', '100%');
        var divContent = jQuery('#gbox_tblGrid');
        divContent.css('-moz-border-radius', '0 0 6px 6px');
        divHeader.click(function () {
            divContent.slideToggle('slow');
            var imgHdr = jQuery('#imgHdrExpand');
            if (imgHdr.attr('src').indexOf('minus-icon.png') == -1) {
                imgHdr.attr('src', imgHdr.attr('src').replace('plus-icon.png', 'minus-icon.png'));
            }
            else {
                imgHdr.attr('src', imgHdr.attr('src').replace('minus-icon.png', 'plus-icon.png'));
            }
        });
        //var theme=$get('ctl00_hdnThemeName').value;
        var title = divHeader.text();
        var titleWidth = getVisualLength(title) + 'px';
        var titleHtml = '<table width="100%" cellpadding="0" cellspacing="0" class="tblFormTitle">' +
                    '<tr><td class="grdVwCurveLeft"><img alt="Spacer" src="' + g_cdnImagesPath + 'Images/spacer.gif" height="1" /></td>' +
                    '<td width="' + titleWidth + '"  align="center" class="grdVwtitle">' + title + '</td>' +
                    '<td class="grdVwCurveRight"><img alt="Spacer" src="' + g_cdnImagesPath + 'Images/spacer.gif" height="1" /></td>' +
                    '<td class="grdVwTitleAuto"><img alt="Spacer" src="' + g_cdnImagesPath + 'Images/spacer.gif" height="1" /></td>' +
                    '<td class="grdVwTitleAuto" style="height: 24px; width: 20px" align="center"><img id="imgHdrExpand" alt="curveRight" src="' + g_cdnImagesPath + 'Images/minus-icon.png" /></td>' +
                    '</tr></table>';
        divHeader.html(titleHtml);
        divHeader.css('cursor', 'pointer');
    }
    //Adjust the grid's width to that of the title.
    var frmTitle = jQuery('.tblFormTitle');
    var bounds;
    if (frmTitle.size() > 0) {
        bounds = Sys.UI.DomElement.getBounds(frmTitle[0]);
    }
    else {
        //Adjust the titles customised to GenProcesEngine.aspx
        bounds = Sys.UI.DomElement.getBounds($get('tblGridContent'));
    }
    objGrid.setGridWidth(bounds.width - 4);
}

function OnPageChange(pg) {
    //    var objGrid=jQuery(gridTableId);
    //    var isInLineEdit = objGrid.jqGrid('getGridParam','isInlineEditable');
    //    if(isInLineEdit)
    //    {
    //        if(g_arrILEModRows && g_arrILEModRows.length>0)
    //        {
    //            var proceed=confirm('You have changes to submit.\n Press Cancel to Submit the changes or Ok to continue.');
    //            if(proceed)
    //            {
    jQuery("#imgbtnInLineSave").hide();
    jQuery("#imgbtnInLineCancel").hide();
    jQuery(editButtonId).attr('disabled', false);
    //            }
    //            else
    //            {
    ////                return false;
    //                throw 'Submit the changes first';
    //            }
    //        }
    //        else
    //        {
    //            jQuery("#imgbtnInLineSave").hide();
    //            jQuery("#imgbtnInLineCancel").hide();
    //            jQuery(editButtonId).attr('disabled',false);
    //        }
    //    }
}

//Does the necessary UI form diplay based on the page and rows count condition etc.,
function DoFormLoad(objGrid, xDocOut) {
    //Form Load.
    var totalRows = objGrid.getGridParam('records');
    if (window.location.href.toUpperCase().indexOf("GENPROCESSENGINE.ASPX") !== -1) {
        var frmParam = "Modify";
        if (totalRows === 0) {
            frmParam = "Add";
        }
        ShowFormOnLoad(frmParam);
    }
    else if (totalRows === 0 && g_IsFirstLoad) {
        ShowAdd();
    }
    else if (totalRows == 1) {
        //Check if the result set was from a search operation.
        var isSearch = xDocOut.getElementsByTagName('search');
        if (isSearch.length === 0 && GetInnerText(isSearch[0]) !== '1') {
            OnSelectImgClick(1);
        }
    }
}

//Adds underline to the sort columns which are identified by the class sortColHdr.
//The style cannot be directly placed in the stylesheet because the same style is getting reflected globally instead
//of the needed headers only.Ex In the View mode.
function StyleSortColumns() {
    jQuery('.sortColHdr').css('text-decoration', 'underline');
}

function ShowAdd() {
    jQuery(addButtonId).trigger('click');
}

function CheckFooterRow() {
    var rowlist = xDocOut.childNodes[0];
    for (i = 0; i < rowlist.childNodes.length; i++) {
        if (rowlist.childNodes[i].nodeName == "TotalRows") {
            totalRows = GetInnerText(rowlist.childNodes[i]);
        }
    }
    if (totalRows == 0) {
        var tr = jQuery(".footrow-ltr");
        for (y = 0; y < tr[0].cells.length; y++) {
            tr[0].cells[y].textContent = "";
        }
    }
}

function InitReportButtons() {
    var divReport = jQuery('#divPager_right');
    divReport.html(jQuery('#divPrintReports').html());

    var arrHvrMns = ['divHvrDirectPrint', 'divHoverPageSelectChart', 'divHoverPageSelectPDF', 'divHoverPageSelectExcel', 'divHoverPageSelectHTML', 'divHoverPageSelectEmailPDF', 'divHvrEmailIDs', 'divHoverPageSelectMSWord'];
    var arrCtrlIds = [];
    arrCtrlIds.push('imgBtnPrint');
    arrCtrlIds.push('imgBtnChart');
    arrCtrlIds.push('imgBtnPDF');
    arrCtrlIds.push('imgBtnExcel');
    arrCtrlIds.push('imgBtnHtml');
    arrCtrlIds.push('imgBtnEmailPDF');
    arrCtrlIds.push('aProcessMenu');
    arrCtrlIds.push('imgBtnMSWord');
    for (var index = 0; index < 8; index++) {
        var objTarget = $get(arrCtrlIds[index]);
        if (objTarget != null) {
            if (objTarget.id == 'aProcessMenu') {
                var objQTip = jQuery(objTarget).qtip(
           {
               content: { text: document.getElementById('divHvrEmailIDs').innerHTML },
               position: { corner: { target: 'bottomMiddle', tooltip: 'topRight' }, adjust: { screen: true} },
               hide: { delay: 20, fixed: true },
               //style: { tip:'topRight',padding: '10px',name: 'grid', border: { width: 2, radius: 6 }},
               style: { tip: 'topRight', padding: '10px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A'} },
               show: { effect: { type: 'slide'} }
           });
            }
            else if (objTarget.id == arrCtrlIds[5]) {
                var objQTip = jQuery(objTarget).qtip(
               {
                   content: { text: $get(arrHvrMns[index]).innerHTML, title: { text: 'E-Mail Report'} },
                   position: { corner: { target: 'topLeft', tooltip: 'bottomRight' }, adjust: { screen: true} },
                   // Make it fixed so it can be hovered over
                   hide: { delay: 100, fixed: true },
                   style: { tip: 'bottomRight', padding: '5px 5px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A'} },
                   show: { effect: { type: 'slide'} }
               });
            }
            else {
                var objQTip = jQuery(objTarget).qtip(
               {
                   content: { text: $get(arrHvrMns[index]).innerHTML, title: { text: ClipPrintType(arrHvrMns[index])} },
                   position: { corner: { target: 'topLeft', tooltip: 'bottomRight' }, adjust: { screen: true} },
                   // Make it fixed so it can be hovered over
                   hide: { delay: 100, fixed: true },
                   style: { tip: 'bottomRight', padding: '5px 5px', name: 'grid', border: { width: 1, radius: 3, color: '#4C708A'} },
                   show: { effect: { type: 'slide'} }
               });
            }
        }
    }
}

function HasToolTip(nodeRow) {
    var arrTTFields = ["Notes", "Attachments", "SecuredBy", "ChangedBy", "ToolTip"];
    var ttNames = new Array();
    for (var i = 0; i < arrTTFields.length; i++) {
        var ttField = nodeRow.getElementsByTagName(arrTTFields[i]);
        if (ttField.length == 1) {
            ttNames.push(arrTTFields[i]);
        }
    }
    return ttNames;
}

function OnSelectImgClick(rowId) {
    EnableGrayOverlay();
    var objGrid = jQuery(gridTableId);
    objGrid.resetSelection();
    objGrid.setSelection(rowId);
    objGrid.viewGridRow(rowId, { drag: false, resize: false, closeOnEscape: true, width: 600, onClose: OnEntryFormClose });
    return false;
}

function GetUrl() {
    var objGrid = jQuery(gridTableId);
    var editUrl = objGrid.getGridParam('url');
    if (editUrl.indexOf('?') != -1) {
        editUrl = editUrl.split('?')[0];
    }
    return editUrl;
}

function ShowStatusMessage(response, postData) {
    var strStatus = (response.responseText).split("-");
    //To check whether record is added successfully or not
    if (strStatus[0] == "Error ") {
        return [false, response.responseText];
    }
    else {
        jqAlert(response.responseText);
        return [true, response.responseText];
    }

}

jQuery(document).ready(function () {

    InitReportButtons();
    var objGrid = jQuery(gridTableId);
    var editUrl = GetUrl();
    var dialogWidth = 600; //objGrid.getGridParam('width');

    //objGrid.navButtonAdd(divPagerName,{caption:'Find',title:'Show Search Form', buttonicon :'ui-icon-search', onClickButton:ShowFormSearch });
    //objGrid.navButtonAdd(divPagerName, { caption: 'Toggle Search', title: 'Toggle Search Toolbar', buttonicon: 'ui-icon-pin-s', onClickButton: function () { ShowSearchToolBar(objGrid, true); } });
    objGrid.navButtonAdd(divPagerName, { caption: 'Reset', title: 'Clear Search', buttonicon: 'ui-icon-refresh', onClickButton: function () { if (typeof (objGrid[0].clearToolbar) === 'function') { objGrid[0].clearToolbar(); } } });
    objGrid.jqGrid('navButtonAdd', divPagerName, { caption: "Columns", title: "Reorder Columns", onClickButton: function () { objGrid.jqGrid('columnChooser', { done: AfterColumnChoose }); } });

    //Add the toggle search button in the header and also change the ID to avoid any undesired changes to the icon by the inherent jqGrid code.
    jQuery('#jqgh_rn').attr('id', 'jqgh_rn_mod').html('<span class="ui-icon" style="background-position:-158px -112px;cursor:pointer"></span>').click(function () { ShowSearchToolBar(objGrid, true); });

    var btnAdd = document.getElementById("btnAdd");
    var btnClone = document.getElementById("btnAddClone");
    var btnModify = document.getElementById("btnModify");
    var btnDelete = document.getElementById("btnDelete");
    var btnFind = document.getElementById("btnAdvancedSearch");
    var btnNote = document.getElementById("btnNote");
    var btnSecure = document.getElementById("btnSecure");
    var btnAttach = document.getElementById("btnAttachment");

    if (btnNote != null) {
        var btnNoteSrc = btnNote.src;
        var btnNoteMouseOver = btnNote.onmouseover;
        var btnNoteMouseOut = btnNote.onmouseout;
    }

    if (btnSecure != null) {
        var btnSecureSrc = btnSecure.src;
        var btnSecureMouseOver = btnSecure.onmouseover;
        var btnSecureMouseOut = btnSecure.onmouseout;
    }

    if (btnAttach != null) {
        var btnAttachSrc = btnAttach.src;
        var btnAttachmMouseOver = btnAttach.onmouseover;
        var btnAttachMouseOut = btnAttach.onmouseout;
    }
    //Add.
    jQuery(addButtonId).click(function () {
        //For Disabling Buttons(Add,Modify etc)
        EnableGrayOverlay();
        objGrid.editGridRow("new",
                                {
                                    reloadAfterSubmit: true,
                                    url: editUrl,
                                    editData: GetReqParams(),
                                    beforeSubmit: function (postdata, formid) {
                                        if (typeof (InitAjaxUpload) == 'function') {
                                            var arrReqVal = GetReqParams();
                                            //AjaxFileUpload Function
                                            InitAjaxUpload();
                                            var Root = FileInfo.activeElement.childNodes;
                                            var fName = GetInnerText(Root[0].firstChild);
                                            var fExtention = GetInnerText(Root[1].firstChild);
                                            var fSize = GetInnerText(Root[2].firstChild);
                                            var items = jQuery(gridTableId).getGridParam('userData');
                                            items["FileName"] = fName;
                                            items["FileExtension"] = fExtention;
                                            items["FileSize"] = fSize;
                                            objGrid.appendPostData({ FileName: items["FileName"] });
                                            objGrid.appendPostData({ FileExtention: items["FileExtension"] });
                                            objGrid.appendPostData({ FileSize: items["FileSize"] });
                                        }
                                        UpdatePostData(postdata);
                                        return [true, 'message'];
                                    },
                                    afterSubmit: ShowStatusMessage,
                                    beforeShowForm: function (rowid) {
                                        //Spacing between last control and the submit button.
                                        jQuery(document.getElementById("FrmGrid_tblGrid")).append("<tr><td style='height:5px'><td></tr>");
                                        jQuery("#SOXAppStatus").attr("disabled", true);
                                    },
                                    afterShowForm: function (rowid) {
                                        //Function to SetFocus to the first 'FormElement'
                                        SetFocus();

                                    },
                                    recreateForm: true,
                                    closeAfterAdd: true,
                                    width: dialogWidth,
                                    closeOnEscape: true,
                                    bottominfo: "Fields marked with (*) are required",
                                    onClose: OnEntryFormClose
                                });
    });

    //AddClone
    jQuery(cloneButtonId).click(function () {
        //For Disabling Buttons(Add,Modify etc)
        EnableGrayOverlay();

        var rowIndex = objGrid.getGridParam('selrow');
        if (rowIndex) {
            rowIndex = parseInt(rowIndex, 10);
            var nodeRow = GetRow(rowIndex);
            var arrReqVal = GetReqParams();
            arrReqVal["selectedRw"] = ConvertToString(nodeRow);
            arrReqVal["status"] = "CLONE";
            objGrid.editGridRow(rowIndex, { editCaption: "Add Clone", clearAfterAdd: false, closeAfterEdit: true, reloadAfterSubmit: true, url: editUrl, editData: arrReqVal,
                afterSubmit: ShowStatusMessage,
                beforeShowForm: function (rowid) {
                    $(document.getElementById("FrmGrid_tblGrid")).append("<tr><td style='height:5px'><td></tr>");
                    jQuery("#SOXAppStatus").attr("disabled", true);
                },
                afterShowForm: function (rowid) {
                    //Function to SetFocus to the first 'FormElement'
                    SetFocus();
                },
                width: dialogWidth,
                recreateForm: true,
                closeOnEscape: true,
                //                                    onClose:DisableOverlay,//For Enabling Buttons(Add,Modify etc)
                bottominfo: "Fields marked with (*) are required",
                onClose: OnEntryFormClose
            });
        }
        else {
            jqAlert("Please Select Row");
        }
    });

    //Add Details
    jQuery(detailsButtonId).click(function () {
        objGrid.editGridRow("new", { reloadAfterSubmit: true, addCaption: "Add Details", clearAfterAdd: true, recreateForm: false, url: editUrl, editData: GetReqParams(),
            afterSubmit: ShowStatusMessage,
            width: 360,
            beforeShowForm: function (rowid) {
                $(document.getElementById("FrmGrid_tblGrid")).append("<tr><td style='height:5px'><td></tr>");
                jQuery("#SOXAppStatus").attr("disabled", true);
            },
            afterShowForm: function (rowid) {
                //Function to SetFocus to the first 'FormElement'
                SetFocus();
            },
            closeOnEscape: true,
            //width:dialogWidth,
            recreateForm: true,
            bottominfo: "Fields marked with (*) are required",
            onClose: OnEntryFormClose
        });
    });

    //Modify
    jQuery(editButtonId).click(function () {
        //The If block consists of the InLineEditing functionality.
        var isInLineEdit = objGrid.jqGrid('getGridParam', 'isInlineEditable');
        if (isInLineEdit) {
            EditGridInline();
        }
        else {
            var rowIndex = objGrid.getGridParam('selrow');
            if (rowIndex) {
                //For Disabling Buttons(Add,Modify etc)
                EnableGrayOverlay();

                rowIndex = parseInt(rowIndex);
                var nodeRow = GetRow(rowIndex);
                var arrReqVal = GetReqParams();
                arrReqVal["selectedRw"] = ConvertToString(nodeRow);
                objGrid.editGridRow(rowIndex, { width: dialogWidth, reloadAfterSubmit: true, url: editUrl, checkOnSubmit: false,
                    beforeSubmit: function (postdata, formid) {
                        //Get the current selected row.
                        var rowid = objGrid.getGridParam('selrow');
                        var row = ConvertToString(GetRow(rowid));
                        postdata.selectedRw = row;
                        UpdatePostData(postdata);
                        return [true, 'message'];
                    },
                    afterSubmit: ShowStatusMessage,
                    onInitializeForm: function (formid) {
                        //For form level links
                        //This should be put in View mode. Because Form level links should appear only in view mode.
                        jQuery("#TblGrid_tblGrid").prepend(CreateBPCLinks(arrReqVal));

                        //To set the input SOXAppStatus class to null
                        document.getElementById("SOXAppStatus").className = '';

                        //To set the image border to zero
                        document.getElementById("SOXAppStatus").style.border = '0px solid';

                        //To Show the SOXApproval Image
                        ShowSoxApprovalImage();
                    },
                    afterclickPgButtons: function (whichbutton, formid, rowid) {
                        //Set the rowid to current row
                        rowIndex = parseInt(rowid, 10);
                        var nodeRow = GetRow(rowIndex);
                        var arrReqVal = GetReqParams();

                        arrReqVal["selectedRw"] = ConvertToString(nodeRow);
                        //To create the Process Links for the selected row.
                        $(document.getElementById("formLinks")).remove();
                        jQuery("#TblGrid_tblGrid").prepend(CreateBPCLinks(arrReqVal));
                        ShowSoxApprovalImage();

                    },
                    beforeShowForm: function (rowid) {
                        $(document.getElementById("FrmGrid_tblGrid")).append("<tr><td style='height:5px'><td></tr>");
                    },
                    afterShowForm: function (rowid) {
                        //Function to SetFocus to the first 'FormElement'
                        SetFocus();
                    },
                    recreateForm: true,
                    closeAfterEdit: true,
                    closeOnEscape: true,
                    //                                        onClose:DisableOverlay,//For Enabling Buttons(Add,Modify etc)
                    bottominfo: "Fields marked with (*) are required",
                    onClose: OnEntryFormClose
                });
            }
            else {
                jqAlert("Please Select Row");
            }
        }
    });

    //Delete
    jQuery(deleteButtonId).click(function () {
        var rowIndex = objGrid.getGridParam('selrow');
        if (rowIndex) {
            rowIndex = parseInt(rowIndex);
            var nodeRow = GetRow(rowIndex);
            var arrReqVal = GetReqParams();
            arrReqVal["selectedRw"] = ConvertToString(nodeRow);
            //delData: This is the data which can be posted to access in delete operation.
            objGrid.delGridRow(rowIndex, {
                width: 400,
                reloadAfterSubmit: true,
                url: editUrl,
                delData: arrReqVal,
                afterSubmit: function (response, postdata) {
                    var strStatus = (response.responseText).split("-");
                    //To check whether record is added successfully or not
                    jqAlert(response.responseText);
                    return [true, ''];
                },
                onclickSubmit: function (params) {
                    var rowIndexDel = objGrid.getGridParam('selrow');
                    rowIndexDel = parseInt(rowIndexDel, 10);
                    var nodeRowDel = GetRow(rowIndexDel);
                    return { selectedRw: ConvertToString(nodeRowDel) };
                },
                beforeShowForm: function (rowid) {
                    jQuery("#SOXAppStatus").attr("disabled", true);
                },
                closeOnEscape: true,
                bottominfo: "Fields marked with (*) are required"
            });
        }
        else {
            jqAlert("Please Select Row");
        }
    });

    //Search Invoke.
    jQuery(searchButtonId).click(function () {
        objGrid.searchGrid({
            multipleSearch: true,
            closeAfterSearch: true,
            searchoptions: { sopt: ['cn', 'bw', 'eq', 'ne', 'lt', 'gt', 'ew'] },
            beforeShowSearch: function () {
                var items = jQuery(gridTableId).getGridParam('userData');
                objGrid.appendPostData({ Node: items["Node"] });
                objGrid.appendPostData({ Find: items["Find"] });
                objGrid.appendPostData({ parentCols: items["parentCols"] });
            },
            closeOnEscape: true
        });
    });


    //Multiple Form Search
    jQuery("#SearchFilter").filterGrid("tblGrid",
        {
            gridModel: false, gridNames: false, formtype: "vertical",
            enableSearch: true, enableClear: true, autosearch: false,
            filterModel: filterModel(),
            afterSearch: function () {
                jQuery(".HeaderButton").trigger("click");
                jQuery("#SearchFilter").css("display", "none");
            },
            beforeSearch: function () {
                var items = objGrid.getGridParam('userData');
                objGrid.appendPostData({ Node: items["Node"] });
                objGrid.appendPostData({ Find: items["Find"] });
                objGrid.appendPostData({ Add: items["Add"] });
                objGrid.appendPostData({ Delete: items["Delete"] });
                objGrid.appendPostData({ Modify: items["Modify"] });
                objGrid.appendPostData({ parentCols: items["parentCols"] });
                objGrid.appendPostData({ filters: "" });
            }
        });

    //Note
    jQuery(noteButtonId).click(function () {
        var rowIndex = objGrid.getGridParam('selrow');
        if (rowIndex) {
            var noteBPGID = document.getElementById("btnNote").value;
            OnBPCCall('', noteBPGID, 'Common/GenView.aspx', '1');
        }
        else {
            jqAlert("Please Select Row");
        }
        return false;
    });

    //Secure
    jQuery(secureButtonId).click(function () {
        var rowIndex = objGrid.getGridParam('selrow');
        if (rowIndex) {
            var secureBPGID = document.getElementById("btnSecure").value;
            OnBPCCall('', secureBPGID, 'Common/GenView.aspx', '1');
        }
        else {
            jqAlert("Please Select Row");
        }

        return false;
    });

    //Attachment
    jQuery(attachButtonId).click(function () {
        var rowIndex = objGrid.getGridParam('selrow');
        if (rowIndex) {
            var attachBPGID = document.getElementById("btnAttachment").value;
            OnBPCCall('', attachBPGID, 'Common/GenView.aspx', '1');
        }
        else {
            jqAlert("Please Select Row");
        }
        return false;
    });

    //Handling Grid Header Button Click
    jQuery(".HeaderButton").click(function () {
        jQuery("#SearchFilter").hide();
    });


    //Decrease the width of the page navigation buttons div to give more room to navButtons
    $get('divPager_left').style.width = '205px';
    $get('divPager_center').style.width = '170px';
    $get('divPager_right').style.width = '170px';

});

//Enable Gray Overlay
function EnableGrayOverlay() {
    jQuery('#divBtnsUC').block({ message: null,
        overlayCSS: {
            border: 'none',
            backgroundColor: 'grey',
            opacity: 0.9,
            zIndex: 949,
            cursor: 'hand',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px'
        }
    });
}

//Disable Gray Overlay
function DisableOverlay() {
    jQuery('#divBtnsUC').unblock();
}

function OnEntryFormClose() {
    DisableOverlay();
}

//function for Multiple Form Search FilterModel 
function filterModel() {
    var arrFilter = new Array();
    var arrFilterModelPrms = ['label', 'name', 'stype', 'defval'];
    var objGrid = jQuery(gridTableId);
    var colModel = objGrid.getGridParam('colModel');
    for (var i = 1; i < colModel.length; i++) {
        var cm = colModel[i];
        if (cm.hidden == false && cm.search == true) {
            var cmFilter = new Object();
            for (var j = 0; j < arrFilterModelPrms.length; j++) {
                var filterPrm = arrFilterModelPrms[j];
                var filterPrm1 = filterPrm;
                if (filterPrm == 'label') {
                    filterPrm1 = 'index';
                }
                var cmPrm = cm[filterPrm1];
                if (cmPrm && cmPrm.length > 0) {
                    if (filterPrm == 'stype' && cmPrm == 'select') {
                        cmFilter['sopt'] = cm.editoptions;
                        cmFilter.surl = "";
                    }

                    if (filterPrm == 'label') {
                        cmFilter[filterPrm] = objGrid.getGridParam('colNames')[i];
                    }
                    else {
                        cmFilter[filterPrm] = cm[filterPrm1];
                    }
                }
            }
            arrFilter.push(cmFilter);
        }
    }
    if (arrFilter.length == 0) {
        return false;
    }
    else {
        return arrFilter;
    }
}

//Show the Form Search invoked from the Grid footer bar.
function ShowFormSearch() {
    //Multiple Form Search
    if (document.getElementById("btnCancel") != null) {
        jQuery("#btnCancel").remove();
    }
    else {   //Setting Properties for Submit Button
        var btnSearch = $get("sButton");
        btnSearch.value = 'Submit';
        btnSearch.className = "fm-button ui-state-default ui-corner-all fm-button-icon-center";
        btnSearch.onclick = 'return false;';
        btnSearch.style.width = '70px';
        btnSearch.style.height = '28px';

        //Setting Properties for Clear Button
        var btnClear = $get("cButton");
        btnClear.className = "fm-button ui-state-default ui-corner-all fm-button-icon-center";
        btnClear.style.width = '70px';
        btnClear.style.height = '28px';

        var objGrid = jQuery(gridTableId);
        var colModel = objGrid.getGridParam('colModel');

        for (var i = 1; i < colModel.length; i++) {
            var cm = colModel[i];

            if (cm.hidden == false && cm.search == true) {
                var inputElement = "sg_" + cm['name'];
                document.getElementById(inputElement).style.width = "210px";
            }
        }
    }

    //Hide or Display the Search Filter div
    var objGrid = jQuery(gridTableId);
    if (jQuery("#SearchFilter").css("display") == "none") {
        if (jQuery(".ui-jqgrid-bdiv").css("display") == "none") {
            jQuery("#SearchFilter").show();
        }
        else {
            jQuery(".HeaderButton").trigger("click");
            jQuery("#SearchFilter").show();
        }
    }

    //Append Cancel Button to the Submit and Clear
    jQuery("#cButton").parent('td').append("<input id='btnCancel' type='button' value='Cancel' class='fm-button ui-state-default ui-corner-all fm-button-icon-center' style='width: 70px; height: 28px;' onclick='javascript:if(CancelClick()==false){return false;}'/>");

    //To Apply CSS Styles to the row and table (to make font bold etc) 
    jQuery("#sButton").parent('td')[0].align = 'left';
    jQuery("#sButton").closest('tr').css('height', '40px');
    jQuery("#sButton").closest('table').css('font-weight', 'bold');
    jQuery("#sButton").closest('table').css('width', '453px');
}

//Handling Cancel Button Click in Multiple Form Search
function CancelClick() {
    jQuery(".HeaderButton").trigger("click");
    jQuery("#SearchFilter").hide();
    return false;
}

//Function to handle the event when a particular row is selected 
function OnSelectRow(rowId, status) {
    //    //In case of multiselect grid remove the Inline edit mode once the row has been unselected.
    //    var objGrid=jQuery(gridTableId);
    //    if(objGrid.getGridParam('multiselect')&&!status)
    //    {
    //        objGrid.restoreRow( rowId)
    //    }

    var nodeRow = GetRow(rowId);
    // Looking for Attachments if Present in the Grid Level
    if (document.getElementById("btnAttachment") != null) {
        //Looking for Attachment if present in the row level
        if (nodeRow.getElementsByTagName("Attachments")[0] != null) {
            //Making Sure the row level Attachment attribute has some text
            if (GetInnerText(nodeRow.getElementsByTagName("Attachments")[0]) != "") {
                document.getElementById("btnAttachment").src = g_cdnImagesPath + "Images/attachment-icon-data.png";
                document.getElementById("btnAttachment").onmouseover = g_cdnImagesPath + "Images/attachment-icon-data-over.png";
                document.getElementById("btnAttachment").onmouseout = g_cdnImagesPath + "Images/attachment-icon-data.png";
            }
            else {
                document.getElementById("btnAttachment").src = g_cdnImagesPath + "Images/attachment-icon.png";
                document.getElementById("btnAttachment").onmouseover = g_cdnImagesPath + "Images/attachment-icon-over.png";
                document.getElementById("btnAttachment").onmouseout = g_cdnImagesPath + "Images/attachment-icon.png";
            }
        }
        else {
            document.getElementById("btnAttachment").src = g_cdnImagesPath + "Images/attachment-icon.png";
            document.getElementById("btnAttachment").onmouseover = g_cdnImagesPath + "Images/attachment-icon-over.png";
            document.getElementById("btnAttachment").onmouseout = g_cdnImagesPath + "Images/attachment-icon.png";
        }
    }

    if (document.getElementById("btnSecure") != null) {
        //Looking for Security if present in the row level
        if (nodeRow.getElementsByTagName("SecuredBy")[0] != null) {
            //Making Sure the row level SecuredBy attribute has some text
            if (GetInnerText(nodeRow.getElementsByTagName("SecuredBy")[0]) != "") {
                document.getElementById("btnSecure").src = g_cdnImagesPath + "Images/security-icon-data.png";
                document.getElementById("btnSecure").onmouseover = g_cdnImagesPath + "Images/security-icon-data-over.png";
                document.getElementById("btnSecure").onmouseout = g_cdnImagesPath + "Images/security-icon-data.png";
            }
            else {
                document.getElementById("btnSecure").src = g_cdnImagesPath + "Images/security-icon.png";
                document.getElementById("btnSecure").onmouseover = g_cdnImagesPath + "Images/security-icon-over.png";
                document.getElementById("btnSecure").onmouseout = g_cdnImagesPath + "Images/security-icon.png";
            }
        }
        else {
            document.getElementById("btnSecure").src = g_cdnImagesPath + "Images/security-icon.png";
            document.getElementById("btnSecure").onmouseover = g_cdnImagesPath + "Images/security-icon-over.png";
            document.getElementById("btnSecure").onmouseout = g_cdnImagesPath + "Images/security-icon.png";
        }
    }

    if (document.getElementById("btnNote") != null) {
        //Looking for Note if present in the row level
        if (nodeRow.getElementsByTagName("Notes")[0] != null) {
            //Making Sure the row level Notes attribute has some text
            if (GetInnerText(nodeRow.getElementsByTagName("Notes")[0]) != "") {
                document.getElementById("btnNote").src = g_cdnImagesPath + "Images/footnote-icon-data.png";
                document.getElementById("btnNote").onmouseover = g_cdnImagesPath + "Images/footnote-icon-data-over.png";
                document.getElementById("btnNote").onmouseout = g_cdnImagesPath + "Images/footnote-icon-data.png";
            }
            else {
                document.getElementById("btnNote").src = g_cdnImagesPath + "Images/footnote-icon.png";
                document.getElementById("btnNote").onmouseover = g_cdnImagesPath + "Images/footnote-icon-over.png";
                document.getElementById("btnNote").onmouseout = g_cdnImagesPath + "Images/footnote-icon.png";
            }
        }
        else {
            document.getElementById("btnNote").src = g_cdnImagesPath + "Images/footnote-icon.png";
            document.getElementById("btnNote").onmouseover = g_cdnImagesPath + "Images/footnote-icon-over.png";
            document.getElementById("btnNote").onmouseout = g_cdnImagesPath + "Images/footnote-icon.png";
        }
    }
    if (document.getElementById("btnDelete") != null) {
        //Looking for OkToDelete if present in the row level
        if (nodeRow.getElementsByTagName("OkToDelete")[0] != null) {
            //Making Sure the row level OkToDelete attribute has some text
            if (GetInnerText(nodeRow.getElementsByTagName("OkToDelete")[0]) != "") {
                if (GetInnerText(nodeRow.getElementsByTagName("OkToDelete")[0]) == "0") {
                    document.getElementById("btnDelete").src = g_cdnImagesPath + "Images/delete-icon-disable.png";
                    document.getElementById("btnDelete").disabled = true;
                    document.getElementById("btnDelete").onmouseover = g_cdnImagesPath + "Images/delete-icon-disable.png";
                    document.getElementById("btnDelete").onmouseout = g_cdnImagesPath + "Images/delete-icon-disable.png";
                }
                else {
                    document.getElementById("btnDelete").src = g_cdnImagesPath + "Images/delete_icon.png";
                    document.getElementById("btnDelete").disabled = false;
                    document.getElementById("btnDelete").onmouseover = g_cdnImagesPath + "Images/delete-icon-over.png";
                    document.getElementById("btnDelete").onmouseout = g_cdnImagesPath + "Images/delete_icon.png";
                }
            }
            else {
                document.getElementById("btnDelete").src = g_cdnImagesPath + "Images/delete_icon.png";
                document.getElementById("btnDelete").disabled = false;
                document.getElementById("btnDelete").onmouseover = g_cdnImagesPath + "Images/delete-icon-over.png";
                document.getElementById("btnDelete").onmouseout = g_cdnImagesPath + "Images/delete_icon.png";
            }
        }
    }
    if (document.getElementById("btnModify") != null) {
        //Looking for OkToDelete if present in the row level
        if (nodeRow.getElementsByTagName("OkToUpdate")[0] != null) {
            //Making Sure the row level OkToDelete attribute has some text
            if (GetInnerText(nodeRow.getElementsByTagName("OkToUpdate")[0]) != "") {
                if (GetInnerText(nodeRow.getElementsByTagName("OkToUpdate")[0]) == "0") {
                    document.getElementById("btnModify").src = g_cdnImagesPath + "Images/modify-icon-disable.png";
                    document.getElementById("btnModify").disabled = true;
                    document.getElementById("btnModify").onmouseover = g_cdnImagesPath + "Images/modify-icon-disable.png";
                    document.getElementById("btnModify").onmouseout = g_cdnImagesPath + "Images/modify-icon-disable.png";
                }
                else {
                    document.getElementById("btnModify").src = g_cdnImagesPath + "Images/modify-icon.png";
                    document.getElementById("btnModify").disabled = false;
                    document.getElementById("btnModify").onmouseover = g_cdnImagesPath + "Images/modify-icon-over.png";
                    document.getElementById("btnModify").onmouseout = g_cdnImagesPath + "Images/modify-icon.png";
                }
            }
            else {
                document.getElementById("btnModify").src = g_cdnImagesPath + "Images/modify-icon.png";
                document.getElementById("btnModify").disabled = false;
                document.getElementById("btnModify").onmouseover = g_cdnImagesPath + "Images/modify-icon-over.png";
                document.getElementById("btnModify").onmouseout = g_cdnImagesPath + "Images/modify-icon.png";
            }
        }
    }
}

//For creating links in the form. This function returns the corresponding links html.
function CreateBPCLinks(arrReqVal, hrefFunc) {
    if (typeof (hrefFunc) === 'undefined') {
        hrefFunc = "OnJQFormLinkClick";
    }
    var strFormLevelLinks = jQuery("#ctl00_cphPageContents_hdnFormLvlLinks").val();
    var arrFormLevelLinks = strFormLevelLinks.split(":");
    var iterateCount = 1;
    var strProcessLinksHtml = "<tr id='formLinks'><td align='right' colspan='2'><table border='0' cellspacing='0' cellpadding='0'>";
    for (i = 0; i < arrFormLevelLinks.length - 1; i++) //ArrayParentChild
    {
        var arrLinkAttr = arrFormLevelLinks[i].split("~");
        //arrLinkAttr[0]--> ID of current BPC node
        //arrLinkAttr[1]--> Label of current BPC node 
        //arrLinkAttr[2]--> BPGID of current BPC node
        //arrLinkAttr[3]--> PageInfo of current BPC node
        //arrLinkAttr[4]--> IsPopup of current BPC node
        if (!ValidateFormLevelLinks(arrReqVal["selectedRw"], arrLinkAttr[0], arrLinkAttr[2], arrLinkAttr[3])) {
            if (i != arrFormLevelLinks.length - 2) {
                if (iterateCount == 1) {
                    strProcessLinksHtml = strProcessLinksHtml + "<td style='width:10px'></td><td align='right'><a oncontextmenu='return false;' Title=" + "'" + arrLinkAttr[1] + "'" + " " + "href=" + "\"" + "javascript:" + hrefFunc + "('" + arrLinkAttr[0] + "'" + "," + "'" + arrLinkAttr[2] + "'" + "," + "'" + arrLinkAttr[3] + "'" + "," + "'" + arrLinkAttr[4] + "'" + ")\">" + arrLinkAttr[1] + "</a></td><td style='width:3px'></td><td>|</td>";
                }
                else {
                    strProcessLinksHtml = strProcessLinksHtml + "<td style='width:10px'></td><td align='center'><a oncontextmenu='return false;' Title=" + "'" + arrLinkAttr[1] + "'" + " " + "href=" + "\"" + "javascript:" + hrefFunc + "(" + "'" + arrLinkAttr[0] + "'" + "," + "'" + arrLinkAttr[2] + "'" + "," + "'" + arrLinkAttr[3] + "'" + "," + "'" + arrLinkAttr[4] + "'" + ")\">" + arrLinkAttr[1] + "</a></td><td style='width:3px'></td><td>|</td>";
                }
            }
            else
                strProcessLinksHtml = strProcessLinksHtml + "<td style='width:10px'></td><td align='center'><a oncontextmenu='return false;' Title=" + "'" + arrLinkAttr[1] + "'" + " " + "href=" + "\"" + "javascript:" + hrefFunc + "(" + "'" + arrLinkAttr[0] + "'" + "," + "'" + arrLinkAttr[2] + "'" + "," + "'" + arrLinkAttr[3] + "'" + "," + "'" + arrLinkAttr[4] + "')\">" + arrLinkAttr[1] + "</a></td>";
            //We can add the link click functionality as href=javascript:OnFormBPCLinkClick(currentBPGID,pageInfo,ID,IsPopup)
            iterateCount = iterateCount + 1;
        }
    }
    strProcessLinksHtml = strProcessLinksHtml + '</tr>';
    return strProcessLinksHtml;
}

//Updates the necessary parameters from the jqGrid's User Data into the PostData.
function UpdatePostData(postData) {
    //Update the AutoFill data from the Text value to the TrxID~TrxType value.
    for (var index in postData) {
        var objPostElement = jQuery(gridTableId.replace('#', '#editmod') + ' #' + index);
        var attAutoFill = objPostElement.attr('autofill');
        if (attAutoFill && attAutoFill.length > 0) {
            var attSplit = attAutoFill.split('~');
            var label = index;
            postData[label + '_TrxID'] = attSplit[0];
            postData[label + '_TrxType'] = attSplit[1];
            //delete postData[label];
        }
    }

    var items = jQuery(gridTableId).getGridParam('userData');
    items = GetTrimmedPostInfo(items); //Remove all the unwanted user data such as Amounts etc
    for (var index in items) {
        postData[index] = items[index];

    }
    return postData;
}

//This function will return array of some default values like treenode,cols info,BPGIDs of Add/Modify/Delete/Find
function GetReqParams() {
    var arrReqVal = new Array();
    var items = jQuery(gridTableId).getGridParam('userData');
    items = GetTrimmedPostInfo(items); //Remove all the unwanted user data such as Amounts etc
    for (var index in items) {
        arrReqVal[index] = items[index];
    }
    return arrReqVal;
}

//jqGrid's Column-Level link click handler.
function OnJQColLinkClick(processName, processBPGID, pageInfo, isPopUp) {
    var objGrid = jQuery(gridTableId);
    objGrid.setSelection(jQuery(g_objLink).closest('tr').attr('id'));

    //The BPC Column Label
    var label = g_objLink.id.split('_')[1];
    var rowId = jQuery(g_objLink).parents('tr .jqgrow').attr('id') || objGrid.getGridParam('selrow');
    //Get the row node
    var nodeRow = GetRow(rowId);
    //As per the logic in GridViewControl.ascx.cs OnRowDataBound.
    var nodeTrxID = GetNode(nodeRow, label + '_TrxID');
    var nodeTrxType = GetNode(nodeRow, label + '_TrxType');
    var TrxID = nodeTrxID ? GetInnerText(nodeTrxID) : GetInnerText(GetNode(nodeRow, 'TrxID'));
    var TrxType = nodeTrxType ? GetInnerText(nodeTrxType) : GetInnerText(GetNode(nodeRow, 'TrxType'));
    var linkText = g_objLink.innerHTML;
    var userData = jQuery(gridTableId).getGridParam('userData');
    var strRow = "<" + userData.Node + "><RowList>" + ConvertToString(PivotXNode(nodeRow)) + "</RowList></" + userData.Node + ">";
    var formInfo = $get('ctl00_cphPageContents_hdnFormInfo').value;
    OnColumnLinkClick(processBPGID, pageInfo, strRow, TrxID, TrxType, formInfo, linkText, isPopUp, processName);
}

//Function to be executed on BPCControls Click (Attachment,Secure,Note)
function OnBPCCall(processName, processBPGID, pageInfo, isPopUp) {
    var objGrid = jQuery(gridTableId);
    var rowId = objGrid.getGridParam('selrow');
    //Get the row node
    var nodeRow = GetRow(rowId);
    var TrxID = GetInnerText(GetNode(nodeRow, 'TrxID'));
    var TrxType = GetInnerText(GetNode(nodeRow, 'TrxType'));
    var linkText = ''; ;
    var userData = jQuery(gridTableId).getGridParam('userData');
    var strRow = "<" + userData.Node + "><RowList>" + ConvertToString(PivotXNode(nodeRow)) + "</RowList></" + userData.Node + ">";
    var formInfo = $get('ctl00_cphPageContents_hdnFormInfo').value;
    OnBPCControlClick(processBPGID, pageInfo, strRow, TrxID, TrxType, formInfo, linkText, isPopUp, processName);
}

//Function to redirect to a page on a FormLevelLink Click
function OnJQFormLinkClick(processName, processBPGID, pageInfo, isPopUp) {
    var objGrid = jQuery(gridTableId);
    var rowId = objGrid.getGridParam('selrow');
    //Get the row node
    var nodeRow = GetRow(rowId);
    var TrxID = GetChildValue(nodeRow, "TrxID");
    var TrxType = GetChildValue(nodeRow, "TrxType");
    var userData = jQuery(gridTableId).getGridParam('userData');
    var linkText = userData.Node;
    var strRow = "<" + userData.Node + "><RowList>" + ConvertToString(PivotXNode(nodeRow)) + "</RowList></" + userData.Node + ">";
    var formInfo = $get('ctl00_cphPageContents_hdnFormInfo').value;
    OnColumnLinkClick(processBPGID, pageInfo, strRow, TrxID, TrxType, formInfo, linkText, isPopUp, processName);
}

//Validate FormLevelLinks Display
function ValidateFormLevelLinks(bpInfo, processName, processBPGID, redirectPage) {
    try {
        if (processName.indexOf("BPGID") != -1) {
            //Get the Row-level BPGID and PageInfo information.
            //bpInfo = PivotXNode(bpInfo);
            var xDocCurrentRow = loadXMLString("<Root>" + bpInfo + "</Root>");
            var nodeRow = xDocCurrentRow.getElementsByTagName("Rows")[0];
            var rowBPGID = GetInnerText(nodeRow.getElementsByTagName(processName)[0]);
            //var rowBPGID=nodeRow.getAttribute(processName);//Row-Level BPGID
            var pageInfoAttName = "PageInfo";
            var BPBIDIndex = parseInt(trim(processName.replace("BPGID", ""))); //Ex:Get the 2 from BPGID2

            if (!isNaN(BPBIDIndex)) {
                //If an index is present
                pageInfoAttName += BPBIDIndex; //Append it to the PageInfo Attribute name.Ex:PageInfo2
            }

            var rowPageInfo = GetInnerText(nodeRow.getElementsByTagName(pageInfoAttName)[0]); //Row-Level PageInfo

            //Priority is to be given to the Row-level info.
            if (rowPageInfo && rowPageInfo.length > 0) {
                redirectPage = rowPageInfo;
            }

            if (rowBPGID && trim(rowBPGID).length > 0) {
                processBPGID = rowBPGID;
            }

            if (!IsURLValid(redirectPage) || (processBPGID == null || trim(processBPGID) == "" || trim(processBPGID) == "0")) {
                return false;
            }
            else {
                return true;
            }
        }
    }
    catch (error) {
        if (g_IsDebugging) {
            jqAlert(error.message);
        }
    }
}

//To Show SOX Approval Image
function ShowSoxApprovalImage() {
    var isSOXBPCExist = jQuery("#ctl00_cphPageContents_hdnSOXApprStatus").val();
    if (isSOXBPCExist != "No SOXAppr") {
        var objGrid = jQuery(gridTableId);
        var rowId = objGrid.getGridParam('selrow');
        if (!rowId) return;
        var nodeRow = GetRow(rowId);
        var strSoxApprovedStatus = GetChildValue(nodeRow, "SoxApprovedStatus");
        var strIsApproved = GetChildValue(nodeRow, "IsApproved");

        if (typeof strSoxApprovedStatus != 'undefined' && strSoxApprovedStatus != null && strSoxApprovedStatus.trim() != "") {
            if (typeof strIsApproved != 'undefined') {
                if (strIsApproved == "1" && strSoxApprovedStatus.length > 0) {
                    document.getElementById("SOXAppStatus").src = g_cdnImagesPath + 'images/Waiting-Approval.png';
                    document.getElementById("SOXAppStatus").setAttribute("title", ('' + strSoxApprovedStatus + '' + '.' + 'Please click to approve'));
                    document.getElementById("SOXAppStatus").setAttribute("onclick", "javascript:if(SOXApprovalStatus('" + isSOXBPCExist + "','" + strSoxApprovedStatus + "')==false){return false;}");
                    document.getElementById("SOXAppStatus").disabled = false;
                }

                if (strIsApproved == "0") {
                    document.getElementById("SOXAppStatus").src = g_cdnImagesPath + 'images/Approved.png';
                    document.getElementById("SOXAppStatus").setAttribute("title", ('' + strSoxApprovedStatus + '' + '.' + 'Please click to disapprove'));
                    document.getElementById("SOXAppStatus").setAttribute("onclick", "javascript:if(SOXApprovalStatus('" + isSOXBPCExist + "','" + strSoxApprovedStatus + "')==false){return false;}");
                    document.getElementById("SOXAppStatus").disabled = false;
                }
            }
            else {
                document.getElementById("SOXAppStatus").src = g_cdnImagesPath + "images/spacer.gif";
                document.getElementById("SOXAppStatus").disabled = true;
            }
        }
        else {
            document.getElementById("SOXAppStatus").src = g_cdnImagesPath + "images/spacer.gif";
            document.getElementById("SOXAppStatus").disabled = true;
        }
    }
    else {
        document.getElementById("SOXAppStatus").disabled = true;
    }
}

//To display the status of SOX Approval(either success or failure)
function SOXApprovalStatus(isSOXBPCExist, strSOXStatus) {
    var objGrid = jQuery(gridTableId);
    var rowId = objGrid.getGridParam('selrow');
    var nodeRow = GetRow(rowId);
    var BPGID = jQuery("#ctl00_cphPageContents_hdnSOXApprStatus").val();

    var strRowXML = ConvertToString(PivotXNode(nodeRow));
    var ProcessStatus = AjaxMethods.SubmitSOXApproval(strRowXML, BPGID, strSOXStatus);
    var statusMessage = ProcessStatus.value.split("~");

    if (statusMessage[0] == "Error") {
        jqAlert(statusMessage[1]);
    }
    else {
        if (document.getElementById("SOXAppStatus").src == g_cdnImagesPath + 'images/Waiting-Approval.png') {
            document.getElementById("SOXAppStatus").src = g_cdnImagesPath + 'images/Approved.png';
            document.getElementById("SOXAppStatus").setAttribute("title", ('' + strSoxApprovedStatus + '' + '.' + 'Please click to disapprove'));
            document.getElementById("SOXAppStatus").setAttribute("onclick", "javascript:if(SOXApprovalStatus('" + isSOXBPCExist + "','" + strSoxApprovedStatus + "')==false){return false;}");
        }

        if (document.getElementById("SOXAppStatus").src == g_cdnImagesPath + 'images/Approved.png') {
            document.getElementById("SOXAppStatus").src = g_cdnImagesPath + 'images/Waiting-Approval.png';
            document.getElementById("SOXAppStatus").setAttribute("title", ('' + strSoxApprovedStatus + '' + '.' + 'Please click to approve'));
            document.getElementById("SOXAppStatus").setAttribute("onclick", "javascript:if(SOXApprovalStatus('" + isSOXBPCExist + "','" + strSoxApprovedStatus + "')==false){return false;}");
        }

        jQuery("#TblGrid_tblGrid").trigger("reloadGrid");
    }

    return false;
}

function cb_SubmitSOXApproval(response) {
    jqAlert(response.value);
}

//Inits(for the first time) and toggles the search toolbar.
function ShowSearchToolBar(jqGridObj, toggle) {
    if (typeof (jqGridObj[0].toggleToolbar) !== 'function') {
        jqGridObj.filterToolbar({ beforeSearch:
                                    function () {
                                        var items = jQuery(gridTableId).getGridParam('userData');
                                        jqGridObj.appendPostData({ Node: items["Node"] });
                                        jqGridObj.appendPostData({ Find: items["Find"] });
                                        jqGridObj.appendPostData({ Add: items["Add"] });
                                        jqGridObj.appendPostData({ Delete: items["Delete"] });
                                        jqGridObj.appendPostData({ Modify: items["Modify"] });
                                        jqGridObj.appendPostData({ parentCols: items["parentCols"] });
                                        jqGridObj.appendPostData({ filters: "" });

                                        //Replace the AutoFill Texts with their corresponding TrxId and TrxTypes
                                        jQuery("tr.ui-search-toolbar input[autofill]").each(
                                        function () {
                                            var afId = jQuery(this).attr('autofill');
                                            if (afId.length > 0) {
                                                var label = GetAutoFillLabel(this);
                                                var trxIDType = afId.split('~');
                                                var trxID = {};
                                                trxID[label + '_TrxID'] = trxIDType[0];
                                                var trxType = {};
                                                trxType[label + '_TrxType'] = trxIDType[1];
                                                jqGridObj.appendPostData(trxID);
                                                jqGridObj.appendPostData(trxType);
                                            }
                                        });
                                    }
        });
    }
    else {
        if (toggle) {
            jqGridObj[0].toggleToolbar();
        }
    }
}

//The RowDataBound event-Called after the row has been bound i.e after the row HTML has been inserted into the DOM.
function OnRowDataBound(rowId, rowData, rowElem) {
    var trRow = jQuery(gridTableId + ' tr[role=row]:eq(' + eval(rowId - 1) + ')');
    InitBPCLinksandImages(rowId, rowData, rowElem, gridTableId);
    var isInLineEdit = jQuery(gridTableId).getGridParam('isInlineEditable');
    if (!isInLineEdit) {
        InitSelectImage(rowId, rowElem, trRow);
    }
    ApplyAlternatingRowStyle(rowId, trRow);
}

//Initialises the select image in the first column and the tooltips for the same, if any.
function InitSelectImage(rowId, nodeRow, trRow) {
    g_SelectIndex = (jQuery(gridTableId).getGridParam('subGrid') === true) ? '2' : '1';
    rowId = parseInt(rowId);
    var imgSelect = document.createElement('input');
    imgSelect.id = 'imgSelectRow' + rowId;
    imgSelect.type = 'image';
    //Check whether the current row has a valid tooltip or not.
    var arrTTFields = HasToolTip(nodeRow);
    var hasToolTip;
    if (arrTTFields.length > 0) {
        hasToolTip = true;
        imgSelect.src = g_cdnImagesPath + 'Images/jqGridInfo2.png';
    }
    else {
        hasToolTip = false;
        imgSelect.title = 'View Details';
        imgSelect.src = g_cdnImagesPath + 'Images/jqGridInfo.png';
    }
    imgSelect.style.width = '19px';
    imgSelect.onclick = function () { return OnSelectImgClick(rowId.toString()); }
    var selectImgTd = trRow.children('td:eq(' + g_SelectIndex + ')');
    selectImgTd.removeAttr('title'); //Remove the empty title attribute as in Chrome&IE
    selectImgTd.attr('align', 'center').append(imgSelect);

    //Init the Qtip here.
    if (hasToolTip) {
        InitToolTipPopup(nodeRow, imgSelect.id, arrTTFields);
    }
}

//Applys the row styles based on the rowId.
function ApplyAlternatingRowStyle(rowId, row) {
    if (parseInt(rowId) % 2 == 0) {
        row.addClass("AlternatingCOARowStyle");
    }
    else {
        row.addClass("GVRowHover");
    }
}

//Intialises the BPC links and Images in a row.
function InitBPCLinksandImages(rowId, rowData, rowElem, tableId) {
    //Show hyperlinks only if IsHyperLinks is true in Preference Values @ server side
    //Get the row updated TrxId from the row following the business logic
    //Get the whther the current process is enabled or not(BPGID3=0/1)
    var objGrid = jQuery(tableId);
    var arrRow = objGrid.getRowData(rowId);
    var arrColumns = objGrid.getGridParam('colModel');
    for (var colCntr = 0; colCntr < arrColumns.length; colCntr++) {
        var col = arrColumns[colCntr];
        var formatter = col.formatter;
        if (formatter && formatter == "showlink") {
            var label = col.xmlmap;
            var jsFunc = col.formatoptions.baseLinkUrl;
            var params = jsFunc.split('(')[1].split(')')[0].split(',');
            var procName = params[0].replace(/'/g, "");
            var processLink = true; //Whether to display the link or not.
            var procNode = GetNode(rowElem, procName); //The node matching the processName
            if (procNode && GetInnerText(procNode) == '0') {
                processLink = false;
            }
            var cellData = objGrid.getCell(rowId, colCntr);
            if (!processLink) {
                //Remove the BPC Link.
                var lnkId = '#lnk' + rowId + '_' + label;
                var objLink = jQuery(lnkId);
                objLink.parent().html(objLink.html());
                objLink.remove();
            }
        }

        //Init the image icon id the column control type is Image.
        var cntrlType = col.edittype;
        if (cntrlType && cntrlType == "image") {
            var colName = col.name;
            if (arrRow[colName].length > 0) {
                objGrid.setRowData(rowId, { ImgSrc: "<img src='" + g_cdnImagesPath + "Images/" + arrRow[colName] + "'></img>" });
            }
        }
    }
}

//Function called after the Columsn are chosen using the Column Chooser.
function AfterColumnChoose(array) {
    if (array) {
        var objGrid = jQuery(gridTableId);
        var colModel = objGrid.getGridParam('colModel');
        g_arrCols = new Array();
        var hdnSelectedCols = $get('ctl00_cphPageContents_hdnSelectedCols');
        for (var i = 0; i < array.length; i++) {
            if (typeof (array[i]) === 'number') {
                var colName = colModel[array[i]].index;
                if (colName) {
                    g_arrCols.push(colName);
                    hdnSelectedCols.value += colName + ':';
                }
            }
        }
        objGrid.jqGrid("remapColumns", array, true);
        //Set the new Select column's index(if modified) into a global variable. 
        var hdrSelect = jQuery('#jqgh_Select').closest('th');
        var allHeaders = hdrSelect.parent().children('th');
        g_SelectIndex = allHeaders.index(hdrSelect);
    }
}

//Function to handle the Frame Loading 
function Openframe(pType, pagename, TrxID, TrxType) {
    //To show loading image
    ShowIframeProgress();
    var pResolution;
    var pHeight;
    var pWidth;
    switch (pType) {
        case "Note":
            var control = document.getElementById("btnNote");
            pResolution = 'height: 380px; width: 600px;';
            var QueryStringValues = "?BPGID=" + control.getAttribute('BPGID') + "&TrxID=" + TrxID + "&TrxType=" + TrxType + "&Resolution=" + pResolution;
            pagename = pagename + QueryStringValues;
            pHeight = '401px';
            pWidth = '600px';
            break;
        case "Attachment":
            var control = document.getElementById("btnAttachment");
            pResolution = 'height: 380px; width: 600px';
            var QueryStringValues = "?BPGID=" + control.getAttribute('BPGID') + "&TrxID=" + TrxID + "&TrxType=" + "&Resolution=" + pResolution;
            pagename = pagename + QueryStringValues;
            pHeight = '401px';
            pWidth = '600px';
            break;
        case "Secure":
            var control = document.getElementById("btnSecure");
            pResolution = 'height: 239px; width: 545px';
            var QueryStringValues = "?BPGID=" + control.getAttribute('BPGID') + "&TrxID=" + TrxID + "&TrxType=" + "&Resolution=" + pResolution;
            pagename = pagename + QueryStringValues;
            pHeight = '260px';
            pWidth = '545px';
            break;
    }
    //Setting Pagename
    var iFrame = $get('iframePage');
    iFrame.src = "../PopUps/" + pagename;
    iFrame.height = pHeight;
    iFrame.width = pWidth;
    ChangeCloseLinkText(pType, this);
    DisplayPopUp(true); //,'ctl00_cphPageContents_BtnsUC_pnlNote');
    return false;
}

var g_objLink;
//Jqgrid inner patch to transfer the link object to the href function at runtime.
function OnLnkClick(obj) {
    g_objLink = obj;
}

function GetChildValue(nodeRow, nodeName) {
    var rows = nodeRow.getElementsByTagName(nodeName);
    if (rows.length > 0) {
        return GetInnerText(rows[0]);
    }
}

//Gets the specified row from the Out XML.
//no is Non-Zero based index of the row position i.e it starts from 1.
function GetRow(no) {
    return xDocOut.getElementsByTagName('Rows')[no - 1];
}

function GetNode(parent, strNode) {
    for (var i = 0; i < parent.childNodes.length; i++) {
        if (parent.childNodes[i].nodeName == strNode) {
            return parent.childNodes[i];
        }
    }
    return null;
}

//Get the text within a given node.
function GetInnerText(node) {
    return node.textContent || node.text;
}

//Converts the ParentNode-ChildNodes format XML to Node-Attributes format.
function PivotXNode(xNodeDoc) {
    var xNode = xNodeDoc.cloneNode(true);
    while (xNode.childNodes.length != 0) {
        var nodeName = xNode.childNodes[0].nodeName;
        var nodeValue = GetInnerText(xNode.childNodes[0]);
        xNode.setAttribute(nodeName, nodeValue);
        xNode.removeChild(xNode.childNodes[0]);
    }
    xNode.removeAttribute("Id");
    return xNode;
}

//Function to Validate DropDownLists in Modal Window
function ValidateDropDown(value, colname) {
    var strValue = value.split("~");

    if (strValue[0] != "-1")
        return [true, ""];
    else
        return [false, colname + ": Field is required"];
}

//Function to create PopUp for BPCControls Click (Attachments,Secure,Note)
function OnBPCControlClick(BPGID, redirectPage, rowInfoClientID, TrxID, TrxType, formInfoClientID, linkText, isPopUp, processName) {
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

    var callingObjXML = "<Object><BPGID>" + formInfoBPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType></Object>";
    redirectPage = "Common/GenView.aspx"; //JQGrid overrride.
    if (isPopUp == "1") {
        redirectPage = "../" + redirectPage + "?PopUp=PopUp&depth=1";
        CommonUI.SetLinkPopUpSession(BPGID, callingObjXML);
        ShowIFrame(redirectPage, { reloadParent: false, isModal: true });
    }
}

//Set all the selected rows to inline-edit mode.
var g_rowCount;
var g_ModCntr = 1;
function EditGridInline() {
    ShowProgress(true);
    var objGrid = jQuery(gridTableId);
    var rowOne = jQuery('tr:eq(0)', objGrid);
    if (rowOne.size() > 0 && rowOne.attr('editable') != '1') {//If rowOne exists and is not in editable mode.
        g_arrILEModRows = new Array();
        g_rowCount = GetRowSetCount(gridTableId);
        for (var i = 1; i <= g_rowCount; i++) {
            setTimeout(function () {
                objGrid.editRow(g_ModCntr, false, OnRowChange);
            }, 20 * i);
        }
    }
    else {
        HideProgress();
    }
    //Show the Save button at the bottom.
    jQuery("#imgbtnInLineSave").show();
    jQuery("#imgbtnInLineCancel").show();
    jQuery(editButtonId).attr('disabled', true);
    //Hide the pager
    ShowPager(false);
}

function OnRowChange() {
    if (g_ModCntr == g_rowCount) {
        HideProgress();
        g_ModCntr = 1;
        //Record the modifications made to a global variable.Attach the events after the controls have been rendered.
        jQuery('td [role=gridcell]').children().change(function () { g_arrILEModRows.push(parseInt(jQuery(this).closest('tr').attr('id'))); });
    }
    else {
        g_ModCntr++;
    }
}

//Save all the inline edited rows to the server.
function SaveInLineEdits() {
    ShowProgress(true);
    var objGrid = jQuery(gridTableId);
    var postInfo = objGrid.getGridParam('userData');
    postInfo = GetTrimmedPostInfo(postInfo);
    postInfo['oper'] = 'edit';
    var arrJqRows = jQuery('tr.jqgrow', objGrid);
    var rowCount = arrJqRows.size();
    //Get the unique array of the modified rows array.
    var arrRows = RemoveDuplicates(g_arrILEModRows);
    //    var arrInvRows=InvertArray(arrRows,rowCount);
    var rowSubmitCntr = 0;
    var noErrors = true;
    for (var i = 0; i < arrRows.length; i++) {
        rowId = arrRows[i];
        var nodeRow = GetRow(rowId);
        var strSelRow = ConvertToString(nodeRow);
        postInfo['selectedRw'] = strSelRow;
        //Check if row is editable or not .
        var jqRow = arrJqRows.eq(rowId - 1);
        var isRowInEditMode = jqRow.attr('editable') == '1' ? true : false;
        if (isRowInEditMode) {
            //Update the PostInfo with the AutoFill attribute for the all the AutoFills in the row.
            jQuery('input:text[autofill]', jqRow).each(
                                        function () {
                                            var label = GetAutoFillLabel(this);
                                            var trxIDType = jQuery(this).attr('autofill').split('~');
                                            postInfo[label + '_TrxID'] = trxIDType[0];
                                            postInfo[label + '_TrxType'] = trxIDType[1];
                                        });
            objGrid.saveRow(rowId,
                            null,
                            null,
                            postInfo,
                            function (id, resp) { //After Save function.
                                if (rowSubmitCntr == arrRows.length - 1) {
                                    jQuery(editButtonId).attr('disabled', false);
                                    objGrid.trigger("reloadGrid");
                                }
                                //alert(resp.responseText);
                                rowSubmitCntr++;
                            },
                            function (id, resp) { //On Error funnction.
                                alert('An error has occurred while trying to save row ' + id + '.' + resp.responseText);
                                if (rowSubmitCntr == arrRows.length - 1) {
                                    jQuery(editButtonId).attr('disabled', false);
                                    objGrid.trigger("reloadGrid");
                                    //HideProgress();
                                }
                                rowSubmitCntr++;
                            },
                            null,
                            function (id) {//OnValidation Error.
                                HideProgress();
                                noErrors = false;
                            }
                          );
        }
        else {
            if (rowSubmitCntr == arrRows.length - 1) {
                jQuery(editButtonId).attr('disabled', false);
                objGrid.trigger("reloadGrid");
            }
            rowSubmitCntr++;
        }

    }
    //    for(var i=0;i<arrInvRows.length;i++)
    //    {
    //        jQuery(gridTableId).saveRow(arrInvRows[i],null,'clientArray',postInfo);
    //    }
    //If there are no server side posts to be made hide the progress soon after Client-side saving is done.
    if (arrRows.length === 0) {
        jQuery(editButtonId).attr('disabled', false);
        objGrid.trigger("reloadGrid");
        //        HideProgress();//Uncomment if the above trigger is commented.
    }
    if (noErrors) {
        jQuery("#imgbtnInLineSave").hide();
        jQuery("#imgbtnInLineCancel").hide();
        ShowPager(true);
    }
}

//Cancels the Inline Edit mode and restores the rows back to normal mode.
function CancelInLineEdits() {
    var objGrid = jQuery(gridTableId);
    ShowProgress(true);
    //    for(var i=1;i<=g_rowCount;i++)
    //    {   setTimeout(function(){
    //        objGrid.restoreRow(g_ModCntr,OnRowChange);
    //        },20*i); 
    //    }

    objGrid.trigger("reloadGrid");
    jQuery("#imgbtnInLineSave").hide();
    jQuery("#imgbtnInLineCancel").hide();
    jQuery(editButtonId).attr('disabled', false);
    ShowPager(true);
}

//state - true to show false to hide
function ShowPager(state) {
    if (state) {
        //jQuery('#divPager_center').show();
        jQuery('#first,#prev,#next,#last').show();
        jQuery('.ui-pg-input').attr('disabled', false);
    }
    else {
        //        jQuery('#divPager_center').hide();
        jQuery('#first,#prev,#next,#last').hide();
        jQuery('.ui-pg-input').attr('disabled', true);
    }

}

//Gets the current number of rowCount displayed in the grid.
function GetRowSetCount(gridID) {
    //    var objGrid=jQuery(gridTableId);
    //    var rowNum=objGrid.getGridParam('rowNum');
    //    if(rowNum==-1) {
    //        return objGrid.getGridParam('records');
    //    }
    //    else {
    //        return rowNum;
    //    }

    //    var rowCount=xDocOut.getElementsByTagName('Rows').length;


    return jQuery(gridID + ' > tbody > tr.jqgrow').size();
}

function GetTrimmedPostInfo(postInfo) {
    var arrPostParams = ['Add', 'Modify', 'Delete', 'Find', 'Node', 'oper', 'parentCols', 'selectedRw'];
    var retObj = new Object();
    for (var i = 0; i < arrPostParams.length; i++) {
        var key = arrPostParams[i];
        var value = postInfo[key];
        if (value != null) {
            retObj[key] = value;
        }
    }
    return retObj;
}

//Gets an Inverted Array of the passed array with the specified size.
function InvertArray(arr, size) {
    var r = new Array();
    o: for (var i = 1; i <= size; i++) {
        for (var x = 0; x < arr.length; x++) {
            if (arr[x] == i) continue o;
        }
        r[r.length] = i;
    }
    return r;
}

function SetFocus() {
    jQuery(".FormGrid .FormElement:input:enabled:first").focus();
}

function log(str) {
    if (typeof (console) != 'undefined') console.log(str);
}

//IncludeJavaScript('../JavaScript/JqCommon.js');

IncludeJavaScript(g_cdnScriptsPath + 'JqCommon.js');
//Includes the given js file into the markup.
function IncludeJavaScript(jsFile) {
    document.write('<script type="text/javascript" src="' + jsFile + '"></script>');
}

