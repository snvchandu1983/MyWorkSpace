//Global Variables For PDF Options
var g_PDFOptions;
var g_TempVariable;

//Contains all the business logic related functions pertaining to JqGrid.
var g_RptInfo;
var g_IsProcEngine = false;
//from jqgrid
function ShowCalc(objID) {
    var myCalc = jQuery('#' + objID);
    if (myCalc.attr('disabled') != true) {
        myCalc.calculator
        ({
            onClose: function (val, obj) { myCalc.calculator('destroy'); CalcClose(val, obj); },
            layout: ['MC_0_._=_+' + jQuery.calculator.CLOSE, 'MR_1_2_3_-' + jQuery.calculator.USE, 'MS_4_5_6_*' + jQuery.calculator.ERASE, 'M+_7_8_9_/']
        });
        var list = document.getElementsByTagName('a');
        list[0].focus();
        var mm = myCalc[0];
        myCalc.focus();
    }
    return false;
}

//from jqgrid
function ShowCalender(elem) {
    //DatePicker trying to intialise the trigger image button to the textbox even before the textbox has been placed in 
    //its destination td element.Hence delaying the attaching process would make the target textbox absolutely ready in the DOM.

    setTimeout(function () {
        if (elem.id) {
            jQuery(elem).datepicker(
            {
                dateFormat: 'mm/dd/y', changeMonth: true, changeYear: true,
                buttonImageOnly: true,
                showOn: 'button',
                buttonImage: g_cdnImagesPath + 'Images/calendar-icon.gif'
            });
            //jQuery(elem).mask('99/99/99');
        }
    }, 100);
}

//from jqgrid
function InitShowCalculator(elem) {
    setTimeout(function () {
        jQuery(elem).calculator(
        {
            buttonImageOnly: true,
            showOn: 'button',
            buttonImage: g_cdnImagesPath + 'calendar-icon.gif'
        });

    }, 100);
}

//Displays the Entry Form on page load for ProcessEngine type pages.
//Called from GenProcessEngine.aspx
function ShowFormOnLoad(frmMode) {
    var frmPrm;
    if (frmMode == "Add") {
        frmPrm = "new";
    }
    else {
        frmPrm = 1;
    }
    g_IsProcEngine = true;
    var objGrid = jQuery(gridTableId);
    objGrid.setSelection(1);
    var dialogWidth = objGrid.getGridParam('width');
    var editUrl = GetUrl();
    //var nodeRow=GetRow(1);
    var arrReqVal = GetReqParams();
    var formOffset = 55;
    var caption = objGrid.getGridParam('caption');
    //arrReqVal["selectedRw"]=ConvertToString(nodeRow);
    //debugger;

    objGrid.editGridRow(frmPrm,
        {
            addCaption: caption,
            editCaption: caption,
            top: -1,
            left: eval(formOffset * -1),
            width: dialogWidth + formOffset,
            reloadAfterSubmit: false,
            url: editUrl,
            resize: false,
            drag: false,
            checkOnSubmit: false,
            closeicon: [false, '', ''],
            beforeSubmit: function (postdata, formid) {
                //Get the current selected row.
                var rowid = 1;
                var nodeRow = GetRow(rowid);
                if (nodeRow) {
                    nodeRow = PivotXNode(nodeRow);
                }
                else {
                    var strRow = "<Root><Rows /></Root>";
                    var xDocRow = loadXMLString(strRow);
                    nodeRow = xDocRow.getElementsByTagName('Rows')[0];
                }
                //Intercept the data submission here and call the js function for report generation and return false to
                //block the conventional submit process.
                //Update the row.
                for (var item in postdata) {
                    var attName = trim(item);
                    if (attName == 'id') { continue; } //Skip if the item is 'id'
                    var attValue = trim(postdata[item]);
                    //                    if(attValue=='No'||attValue=='off'){ attValue='0'; }
                    //                    else if(attValue=='Yes'||attValue=='on'){ attValue='1'; }

                    //Get the Element mapped to the current postdata item.
                    var elem = $get(attName);
                    //Check if the element is an AutoFill, if so get the selected items if any.
                    var autoFillID = elem ? elem.getAttribute('autofill') : '';
                    var isAutoFill = false;
                    if (autoFillID != null) {
                        isAutoFill = true;
                        if (autoFillID.length > 0 && elem.value.length > 0) {
                            attValue = autoFillID;
                        }
                        else {
                            //If nothing is selected via the autofill default the TrxID and TrxTypes to -1~n
                            var attTrxType = nodeRow.getAttribute(attName + '_TrxType');
                            if (attTrxType) {
                                attValue = '-1~' + attTrxType;
                            }
                        }
                    }

                    if (attValue.indexOf('~') != -1) { //For Drop Downs
                        var split = attValue.split('~');
                        //If the value contains only <numeral~> then that numeral is a ReportBPGID.
                        if (split[1].length == 0 || isNaN(split[1])) {
                            //Override the global varaible g_RptInfo with the current value.
                            g_RptInfo[1] = split[0];
                        }
                        else {
                            //Don't send items with TrxID -1
                            if (split[0] != -1) {
                                nodeRow.setAttribute(attName + '_TrxID', split[0]);
                                nodeRow.setAttribute(attName + '_TrxType', split[1]);
                            }
                        }
                    }
                    else { //Regular controls

                        if (trim(attValue).length != 0) {
                            nodeRow.setAttribute(attName, attValue);
                        }
                    }
                }
                //Set the attribute the BPAction as Report for the request row.
                nodeRow.setAttribute("BPAction", "Report");

                var strRow = ConvertToString(nodeRow);
                var TrxID = nodeRow.getAttribute('TrxID') || '';
                var TrxType = nodeRow.getAttribute('TrxType') || '';
                var userData = jQuery(gridTableId).getGridParam('userData');
                strRow = "<" + userData.Node + "><RowList>" + strRow + "</RowList></" + userData.Node + ">";
                var formInfo = $get('ctl00_cphPageContents_hdnFormInfo').value;
                var formInfoBPGID = formInfo.split('~::~')[0];
                var formInfoPage = formInfo.split('~::~')[1];
                var callingObjXML = "<CallingObject><BPGID>" + formInfoBPGID + "</BPGID><TrxID>" + TrxID + "</TrxID><TrxType>" + TrxType + "</TrxType><PageInfo>" + formInfoPage + "</PageInfo><Caption></Caption></CallingObject>";
                var COXML = strRow + callingObjXML;
                //Get the Report Type form the radio button.
                var rptType = (jQuery("input[name=RptType]:radio:checked").val() == "PDF") ? 1 : 2;
                //g_RptInfo[2]=g_RptInfo[2]+"?PgNo="+g_PDFOptions.PageNumber+"&WterMark="+g_PDFOptions.WaterMark+"&PgLyOut="+g_PDFOptions.PageLayout+"&PgSize="+g_PDFOptions.PageSize+"&FntName="+g_PDFOptions.FontName+"&Pwd="+g_PDFOptions.Password+"&Annot="+g_PDFOptions.Annotations;
                CallBPCProcess(g_RptInfo[1], GetRedirectPage(g_RptInfo[2]), g_RptInfo[0], g_RptInfo[3], COXML, rptType);
                jQuery('.ui-state-error').hide();
                return [false, 'Request Succesfully sent.'];
            },
            afterSubmit: function (response, postdata) {
                var strStatus = '';
                strStatus = (response.responseText).split("-");
                //To check whether record is added successfully or not
                if (strStatus[0] == "Error ") {
                    return [false, response.responseText];
                }
                else {
                    jqAlert(response.responseText);
                    return [true, ''];
                }
            },
            onInitializeForm: function (formid) {
                //For form level links
                //This should be put in View mode. Because Form level links should appear only in view mode.
                jQuery("#TblGrid_tblGrid").prepend(CreateBPCLinks(arrReqVal, 'OnReportLinkClick')); //Custom link creation..
                //To set the input SOXAppStatus class to null
                document.getElementById("SOXAppStatus").className = '';
                //To set the image border to zero
                document.getElementById("SOXAppStatus").style.border = '0px solid'
                //To Show the SOXApproval Image
                ShowSoxApprovalImage();

                SetDescription(formid);
            },
            afterShowForm: function (rowid) {
                //Function to SetFocus to the first 'FormElement'
                SetFocus();
                jQuery('.ui-jqdialog-titlebar-close').remove(); //Remove the close dialog button.
                jQuery('#ctl00_cphPageContents_pnlBtns').css('visibility', 'hidden'); //Hide the BtnsUC panel.

                //Create the RadioButton group for Report Type selection.
                //Create the container td
                var theme = $get('ctl00_hdnThemeName').value;
                var tdMain = document.createElement('td');
                tdMain.align = 'right';
                var table = document.createElement('table');
                var tr = document.createElement('tr');
                table.appendChild(tr);

                var td = document.createElement('td');
                td.valign = 'middle';
                //                td.innerHTML+="<table><tr><td><input type='radio' value='PDF' name='RptType' checked/></td>"+
                //                            "<td><img src='../App_Themes/"+theme+"/Images/grid-pdf.png' alt='Print PDF' id='imgPDF'); /></td></tr></table>";             
                td.innerHTML += "<table><tr><td><input type='radio' value='PDF' name='RptType' checked/></td>" +
                            "<td style='cursor:pointer'><img src='" + g_cdnImagesPath + "Images/grid-pdf.png' alt='Print PDF' id='imgPDF'  onclick=javascript:ShowPDFPopUp(\'divAdvPDF\'); /></td></tr></table>";
                tr.appendChild(td);
                td = document.createElement('td');
                td.valign = 'middle';
                td.innerHTML += "<table><tr><td><input type='radio' value='Excel' name='RptType'/></td>" +
                            "<td><img src='" + g_cdnImagesPath + "Images/grid-excel.png' alt='Print Excel' title='Select Excel'/></td></tr></table>";
                tr.appendChild(td);
                tdMain.appendChild(table);
                //Hide the Submit/Cancel buttons
                jQuery('#Act_Buttons td').css('display', 'none');
                jQuery('#Act_Buttons').append(tdMain);
                //QTip For Image PDF -- Added By Chandu
                jQuery('#imgPDF').qtip
                ({
                    content: 'Click for Advanced PDF Options',
                    show: 'mouseover',
                    hide: 'mouseout',
                    position: { corner: { target: 'topLeft', tooltip: 'bottomRight' }, adjust: { screen: true} },
                    style: { background: 'white', 'font-weight': 'bold', tip: 'bottomRight', padding: 2, name: 'grid', border: { width: 1, radius: 2} }
                });
            },
            recreateForm: false,
            closeAfterEdit: false,
            closeOnEscape: false
        });
}
function ShowPDFPopUp(objDivID) {
    CheckPDFOptions();
    var jqObj = jQuery('#' + objDivID);
    jqObj.dialog('destroy');
    var jqDialog = jqObj.dialog({ width: 320, height: 260, modal: true, draggable: true, resizable: false, closeOnEscape: true, title: 'Advanced PDF Options',
        close: function () { CheckPDFOptions(); },
        buttons: {
            "Close": function () { ClosePDFOptions(); jQuery(this).dialog("close"); },
            "Reset": function () { ResetPDFOptions(); },
            "Ok": function () { SetPDFOptions(); jQuery(this).dialog("close") }
        }
    });
    jqObj.removeClass("ui-dialog-content ui-widget-content")
}

function CheckPDFOptions() {
    if (g_PDFOptions == null) {
        g_PDFOptions = new Object();
    }
    g_PDFOptions.PageNumber = jQuery('#chkPageNo').is(':checked');
    g_PDFOptions.WaterMark = jQuery('#chkWaterMark').is(':checked');
    g_PDFOptions.PageLayout = jQuery('#ddlPageLayout').find(":selected").val();
    g_PDFOptions.PageSize = jQuery('#ddlPageSize').find(":selected").val();
    g_PDFOptions.FontName = jQuery('#ddlFontName').find(":selected").val();
    g_PDFOptions.Password = jQuery('#txtPassword').val();
    g_PDFOptions.Annotations = jQuery('#chkAnnotations').is(':checked');
}

function SetPDFOptions() {
    CheckPDFOptions();
    //Declaring Global Variables
    g_TempVariable = jQuery('#chkPageNo').is(':checked');
    g_PDFOptions.PageNumber = g_TempVariable;
    if (g_TempVariable == true) { jQuery('#chkPageNo').attr('checked', true); }
    else { jQuery('#chkPageNo').attr('checked', false); }
    //
    g_TempVariable = jQuery('#chkWaterMark').is(':checked');
    g_PDFOptions.WaterMark = g_TempVariable;
    if (g_TempVariable == true) { jQuery('#chkWaterMark').attr('checked', true); }
    else { jQuery('#chkWaterMark').attr('checked', false); }
    //
    g_TempVariable = jQuery('#ddlPageSize').find(":selected").val();
    jQuery("#ddlPageLayout option[value='" + g_TempVariable + "']").attr("selected", "selected");
    g_PDFOptions.PageSize = g_TempVariable;
    //
    g_TempVariable = jQuery('#ddlPageSize').find(":selected").val();
    jQuery("#ddlPageSize option[value='" + g_TempVariable + "']").attr("selected", "selected");
    g_PDFOptions.PageSize = g_TempVariable;
    //
    g_TempVariable = jQuery('#ddlFontName').find(":selected").val();
    jQuery("#ddlFontName option[value='" + g_TempVariable + "']").attr("selected", "selected");
    g_PDFOptions.FontName = g_TempVariable;
    //
    g_TempVariable = jQuery('#txtPassword').val();
    g_PDFOptions.Password = g_TempVariable;
    //
    g_TempVariable = jQuery('#chkAnnotations').is(':checked');
    g_PDFOptions.Annotations = g_TempVariable;
    if (g_TempVariable == true) { jQuery('#chkAnnotations').attr('checked', true); }
    else { jQuery('#chkAnnotations').attr('checked', false); }
    //  
}

function ResetPDFOptions() {
    jQuery('#chkPageNo').attr('checked', true);
    jQuery('#chkWaterMark').attr('checked', false);
    jQuery("#ddlPageLayout option[value='Portrait']").attr("selected", "selected");
    jQuery("#ddlPageSize option[value='Letter']").attr("selected", "selected");
    jQuery("#ddlFontName option[value='Arial']").attr("selected", "selected");
    jQuery('#txtPassword').val(' ');
    jQuery('#chkAnnotations').attr('checked', true);
    //
    g_PDFOptions.WaterMark = jQuery('#chkWaterMark').is(':checked');
    g_PDFOptions.PageLayout = jQuery('#ddlPageLayout').find(":selected").val();
    g_PDFOptions.PageSize = jQuery('#ddlPageSize').find(":selected").val();
    g_PDFOptions.FontName = jQuery('#ddlFontName').find(":selected").val();
    g_PDFOptions.Password = jQuery('#txtPassword').val();
    g_PDFOptions.Annotations = jQuery('#chkAnnotations').is(':checked');
    //
}

function ClosePDFOptions() {
    g_TempVariable = g_PDFOptions.PageNumber;
    if (g_TempVariable == true) { jQuery('#chkPageNo').attr('checked', true); }
    else { jQuery('#chkPageNo').attr('checked', false); }
    //
    g_TempVariable = g_PDFOptions.WaterMark;
    if (g_TempVariable == true) { jQuery('#chkWaterMark').attr('checked', true); }
    else { jQuery('#chkWaterMark').attr('checked', false); }
    //
    g_TempVariable = g_PDFOptions.PageLayout;
    jQuery("#ddlPageLayout option[value='" + g_TempVariable + "']").attr("selected", "selected");
    //
    g_TempVariable = g_PDFOptions.PageSize;
    jQuery("#ddlPageSize option[value='" + g_TempVariable + "']").attr("selected", "selected");
    //
    g_TempVariable = g_PDFOptions.FontName;
    jQuery("#ddlFontName option[value='" + g_TempVariable + "']").attr("selected", "selected");
    //
    g_TempVariable = jQuery('#txtPassword').val();
    //
    g_TempVariable = g_PDFOptions.Annotations;
    if (g_TempVariable == true) { jQuery('#chkAnnotations').attr('checked', true); }
    else { jQuery('#chkAnnotations').attr('checked', false); }
    //  
}

//Extract the Description text which comes as a textbox control and put it into a label.
function SetDescription(frmID) {
    var leftTd = jQuery('td.CaptionTD:contains(Description)', frmID); //.css('border','1px solid red')
    var rightTdTxtBox = jQuery(leftTd).siblings().children('input');
    var descText = rightTdTxtBox.val();
    rightTdTxtBox.remove();

    leftTd.css('height', '40px').css('vertical-align', 'top');
    leftTd.removeClass('CaptionTD').addClass('mbodyb');
    leftTd.attr('colspan', 2).attr('align', 'left');
    leftTd.html('<span>' + descText + '</span>');
}

//Called on the process-link click event of BPC links.
function OnReportLinkClick(processName, processBPGID, pageInfo, isPopUp) {
    SetPDFOptions();
    g_RptInfo = new Array();
    g_RptInfo.push(processName);
    g_RptInfo.push(processBPGID);
    g_RptInfo.push(pageInfo);
    g_RptInfo.push(isPopUp);
    jQuery('.fm-button-icon-left').trigger('click');
}

//*******************************************************************************************************************
//*************************************JQGrid AutoFills Section******************************************************
//*******************************************************************************************************************

function GetAutoFillLabel(elem) {
    var label = elem.name; //Holds good for AutoFills in the Search Toolbar.
    if (!label || trim(label).length == 0) {
        var idSplit = elem.id.split('_');
        if (isNaN(idSplit[0])) {
            label = elem.id;
        }
        else {
            label = idSplit[1];
        }
    }
    return label;
}

//Attaches a AutoFill plugin and adds an attribute "autofill" so that TrxID and TrxType are sent when the form is 
//submitted. Called when Ctype="TBox" and IsLink="1".
function AttachAutoFill(elem) {
    if (elem.type == 'text') {
        //Add an attribute to the text element which identifies that it is an AutoFill.
        elem.setAttribute('autofill', '');

        //Update the TrxID and TrxType of the pre-existing value.
        if (trim(elem.value).length > 0) {
            var objGrid = jQuery(gridTableId);
            var label = GetAutoFillLabel(elem);
            var isInLineEdit = objGrid.jqGrid('getGridParam', 'isInlineEditable');
            //Get the parent Row Index to get the corresponding XML row.
            var parentRowId;
            if (typeof (g_SubGrid) === 'object' && typeof (g_SubGrid.parentRow) !== 'undefined') {
                parentRowId = parseInt(g_SubGrid.parentRow, 10);
            }
            else if (isInLineEdit) {
                parentRowId = parseInt(elem.id.split('_')[0]);
            }
            else {
                parentRowId = objGrid.getGridParam('selrow');
            }
            //            Log('Attach AutoFill Parent row : ' + parentRowId);
            var xNodeRow = GetRow(parentRowId);
            var afTrxID = GetInnerText(GetNode(xNodeRow, label + '_TrxID'));
            var afTrxType = GetInnerText(GetNode(xNodeRow, label + '_TrxType'));
            elem.setAttribute('autofill', afTrxID + '~' + afTrxType);
        }
        //Attach an onchange function to the AF to set the TrxIDType to -1 if the text has been cleared.
        jQuery(elem).change(function () {
            var el = jQuery(elem);
            if (el.val().length == 0) {
                el.attr('autofill', '-1~' + el.attr('autofill').split('~')[1]);
            }
        });

        AttachAutoFillExt(elem, false);
    }
}

function AttachAutoFillExt(elem, allowMultiple) {
    if (elem.type == 'text') {
        var ctxKey = GetAutoFillLabel(elem);
        elem.className = 'LajitTextBox';
        //Clear the 'AutoFill' attribute contents of the TextBox have been cleared.
        //elem.onchange=function(){ if(elem.value.length==0)elem.setAttribute('autofill',''); };
        //jQuery(elem).autocomplete('../HttpHandlers/AutoFillHandler.ashx', {
        jQuery(elem).autocomplete(g_virtualPath + 'HttpHandlers/AutoFillHandler.ashx', {
            width: 300,
            multiple: allowMultiple,
            matchContains: true,
            scrollHeight: 300,
            max: 900,
            cacheLength: 0,
            minChars: 1,
            extraParams: { contextkey: ctxKey,
                COA: function () {
                    //Set COA attribute from hdnJobCOADefault hiddenbox
                    var currCOA = '';
                    if ((elem.id == "AutoFillAccount") || (elem.id == "AutoFillAccount_1") || (elem.id == "AutoFillAccount_2")) {
                        currCOA = jQuery('#ctl00_cphPageContents_hdnJobCOADefault').val();
                    }
                    return currCOA;
                }
                                        , multiple: allowMultiple ? '1' : '0'
            }, //extraParams close
            formatItem: FormatItem,
            formatResult: function (item) { return FormatResult(item, allowMultiple); }
        }).result(function (event, item)//OnItemSelected Event.
        {
            var isAF = elem.getAttribute("autofill");
            if (typeof (isAF) == "string") {
                elem.setAttribute("autofill", item[0]); //Changed from 1
            }
            //Update the JOB COA if the AutoFillJob has been selected.
            if (ctxKey.startsWith('AutoFillJob')) {
                jQuery('#ctl00_cphPageContents_hdnJobCOADefault').val(item[2]);
            }
        });
    }
}

function FormatItem(item) {
    return item[1]; //0
}

function FormatResult(item, isMultiple) {
    //AutofillID | Text | [COA/Owed/1099/helpBPGID] | CommaValue
    if (isMultiple) {
        return item[3]; //CommaValue is always at the 4th place.
    }
    else {
        return item[1]; //0
    }
}

//*******************************************************************************************************************
//*****************************************JQGrid AutoFills Section End**********************************************
//*******************************************************************************************************************


//*********************************************SubGrid Section*******************************************************
var subgrid_table_id;
var g_SubGridMods = {};
function OnSubGridBeforeExpand(subgrid_id, row_id) {
    var jqRow = jQuery(gridTableId + ' > tbody > .jqgrow[role=row] ').eq(parseInt(row_id - 1, 10));
    var isParentRowInEditMode = jqRow.attr('editable') == '1' ? true : false;
    if (isParentRowInEditMode) {
        jqAlert('Please exit the Inline mode by saving the grid before spliting the transaction');
        return false;
    }
    else {
        return true;
    }
}

function OnSubGridExpanded(subgrid_id, row_id) {
    // we pass two parameters 
    // subgrid_id is a id of the div tag created whitin a table data 
    // the id of this elemenet is a combination of the "sg_" + id of the row 
    // the row_id is the id of the row 
    // If we wan to pass additinal parameters to the url we can use
    // a method getRowData(row_id) - which returns associative array in type name-value 
    // here we can easy construct the following 

    //Set the necessary values of SubGrid var
    g_SubGrid.parentRow = row_id;

    var objGrid = jQuery(gridTableId);
    subgrid_table_id = subgrid_id + "_t";
    var pager_id = "p_" + subgrid_table_id;

    //Convert the IDs to Jquery friendly format.
    subgrid_id = "#" + subgrid_id;

    //Inject the container table for the Subgrid.
    $(subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");

    subgrid_table_id = '#' + subgrid_table_id;

    //Also inject the Save and Cancel buttons for the SubGrid.
    var tblButtons = GetBtnsTable(objGrid, jQuery(subgrid_table_id), row_id);
    $(subgrid_id).append(tblButtons);

    //Remove the undesired Columns such as RowNumber(rn) and subgrid.
    var colNames = objGrid.getGridParam('colNames');
    var sgColNames = [];
    for (var i = 3; i < colNames.length; i++) {
        sgColNames[i - 3] = colNames[i];
    }

    var colModel = objGrid.getGridParam('colModel');
    var sgColModel = [];
    
    for (var i = 3; i < colModel.length; i++) {
        sgColModel[i - 3] = {};
        jQuery.extend(true, sgColModel[i - 3], colModel[i]);
//        sgColModel[i - 3] = colModel[i];
        //Remove the format options for date fields as the date is already formatted.
        var fmtr = sgColModel[i - 3].formatter;
        var fmtOpts = sgColModel[i - 3].formatoptions;
        if (fmtr && fmtr == 'date' && typeof (fmtOpts) == 'object') {
            delete sgColModel[i - 3].formatter;
            delete sgColModel[i - 3].formatoptions;
        }
        if (colModel[i].cType === 'SCalc') {
            delete sgColModel[i - 3].editoptions.disabled; //Re Enable the display-only fields.
        }
    }

    var sgRow = GetSGRowData(objGrid, row_id); //Get the current expanded parent row's data.
    var rowData = sgRow[0];
    var sCalcCols = GetSCalcCols(objGrid);
    //Get the amount/value from the parent row which has to be balanced.
    var sCalcCol = sCalcCols[0];
    var valToBalance = sgRow[1][sCalcCol];
    var sgTitle = 'Split Transaction Value : <span style="font-size:13px;color:#003063">' + valToBalance + '</span>';

    //Intialise the inputs.
    var jqInputs = {
        url: objGrid.getGridParam('url'),
        datatype: "local",
        colNames: sgColNames,
        colModel: sgColModel,
        rowNum: 5,
        height: 130,
        isInlineEditable: true,
        editurl: objGrid.getGridParam('editurl'),
        viewrecords: false,
        sortorder: objGrid.getGridParam('sortorder'),
        caption: sgTitle,
        loadComplete: SubGridLoadComplete,
        onSelectRow: OnSubGridSelectRow,
        footerrow: true,
        userDataOnFooter: false,
        afterInsertRow: OnSubGridRowDataBound,
        xmlReader: { root: 'RowList',
            row: 'Rows',
            page: 'RowList>PageNo',
            total: 'RowList>Pages',
            records: 'RowList>TotalRows',
            repeatitems: false,
            id: 'Id'
        }
    };
    var objSubGrid = jQuery(subgrid_table_id).jqGrid(jqInputs);
    jQuery(subgrid_table_id).jqGrid('navGrid', "#" + pager_id, { edit: false, add: false, del: false });

    //Add the 5 rows in the Sub grid.
    for (var i = 1; i <= 5; i++) {
        objSubGrid.addRowData(i, rowData, 'last', 1);
        var trRow = jQuery(subgrid_table_id + ' tr[role=row]:eq(' + eval(i - 1) + ')');
        ApplyAlternatingRowStyle(i, trRow);
    }

    //Set the footer values for SCalc columns.
    var footerData = {};
    for (var i = 0; i < sCalcCols.length; i++) {
        footerData[sCalcCols[i]] = 0;
    }
    objSubGrid.footerData('set', footerData);

    //Make the added rows editable.
    var sgRowCount = GetRowSetCount(subgrid_table_id);
    for (var i = 1; i <= sgRowCount; i++) {
        objSubGrid.editRow(i, false, function (rId) {
            for (var i = 0; i < sCalcCols.length; i++) {
                var cellElement = jQuery('#' + rId + '_' + sCalcCols[i], objSubGrid);
                cellElement.attr('autocomplete', 'off');
                cellElement.bind('change', { scCol: sCalcCols[i] }, function (ev) {
                    //Update the SubGrid footer when the SCalc column elements are changed.
                    UpdateFooterSCalcs(ev.data.scCol, objSubGrid);
                });
            }
        });

    }

    //Record the modifications made to a global variable.Attach the events after the controls have been rendered.
    jQuery('td[role=gridcell]:visible', objSubGrid).children().change(function () {
        var arrSGMods = (g_SubGridMods[row_id] == null) ? [] : g_SubGridMods[row_id];
        arrSGMods.push(parseInt(jQuery(this).closest('tr').attr('id')));
        g_SubGridMods[row_id] = arrSGMods;
    });

}

//Updates the footer value of the passed column when the column is changed.
function UpdateFooterSCalcs(scCol, objSubGrid) {
    var sgId = objSubGrid.attr('id');
    var rowCount = GetRowSetCount('#' + sgId);
    var colSum = 0;
    for (var i = 1; i <= rowCount; i++) {
        var elVal = jQuery('#' + i + '_' + scCol, objSubGrid).val();
        colSum += parseFloat(elVal);
    }
    var footerData = {};
    footerData[scCol] = colSum;
    objSubGrid.footerData('set', footerData);
}

function OnSubGridRowDataBound(rowId, rowData, rowElem) {
    var trRow = jQuery(subgrid_table_id + ' tr[role=row]:eq(' + eval(rowId - 1) + ')');
    InitBPCLinksandImages(rowId, rowData, rowElem, subgrid_table_id);
    ApplyAlternatingRowStyle(rowId, trRow);
}

//Gets the row data of the parent grid to be bound to the sub-grid.
//This method also blanks out the SCalc fiels in the row data.
function GetSGRowData(objGrid, row_id) {
    var objRowData = objGrid.getRowData(row_id);
    var arrSCalcCols = GetSCalcCols(objGrid);
    var clearedVals = {}; //Stores the values which were reset to zero.
    for (var i = 0; i < arrSCalcCols.length; i++) {
        var sCalcCol = arrSCalcCols[i];
        clearedVals[sCalcCol] = objRowData[sCalcCol];
        objRowData[sCalcCol] = '0.00';
    }
    return [objRowData, clearedVals];
}

//Verifies whether the sum total of the SCalc column in the sub-grid matches the same in the parent grid.
function DoVerifications(objGrid, objSubGrid, colSCalc, parentRowId) {
    var parentRow = objGrid.getRowData(parentRowId);
    var parentSCalcVal = (parentRow) ? parseFloat(parentRow[colSCalc]) : '';
    var sgRowCount = objSubGrid.getGridParam('rowNum');
    var sgSumTotal = 0;
    for (var i = 1; i <= sgRowCount; i++) {
        var rowData = objSubGrid.getRowData(i);
        var amt = parseFloat(jQuery('#' + i + '_' + colSCalc, objSubGrid).val());
        sgSumTotal += amt;
    }
    return (sgSumTotal === parentSCalcVal) ? true : false;
}

//Returns the array of SCalc control type columns in  the ColModel of the passed grid object.
function GetSCalcCols(objGrid) {
    var colModel = objGrid.getGridParam('colModel');
    var arrSCalcs = [];
    for (var i = 1; i < colModel.length; i++) {
        var cm = colModel[i];
        if (cm.cType === 'SCalc') {
            arrSCalcs.push(cm.index);
        }
    }
    return arrSCalcs;
}

function SaveSGEdit(objGrid, objSubGrid, parentRowId) {
    var arrSCalcCols = GetSCalcCols(objGrid);
    var isOk = true;
    var validationErrorStr = '';
    for (var i = 0; i < arrSCalcCols.length; i++) {
        var colSCalc = arrSCalcCols[i];
        isOk = isOk && DoVerifications(objGrid, objSubGrid, colSCalc, parentRowId);
        if (!isOk) {
            validationErrorStr = colSCalc + ' not matching!!';
            break
        };
    }
    if (isOk) {
        ShowProgress(true);
        var postInfo = objGrid.getGridParam('userData');
        postInfo = GetTrimmedPostInfo(postInfo);
        postInfo['oper'] = 'add';
        var arrJqRows = jQuery('tr.jqgrow', objSubGrid);
        var rowCount = arrJqRows.size();

        if (typeof (g_SubGridMods[parentRowId]) === 'undefined') {
            alert('No changes made');
            HideProgress();
            return false; //Probably goto collapse subgrid section.
        }

        //Get the unique array of the modified rows array.
        var arrRows = RemoveDuplicates(g_SubGridMods[parentRowId]);
        var rowSubmitCntr = 0;
        var noErrors = true;
        for (var i = 0; i < arrRows.length; i++) {
            rowId = arrRows[i];
            var nodeRow = GetRow(parentRowId); //rowId
            var strSelRow = ConvertToString(nodeRow);
            //postInfo['selectedRw'] = strSelRow;
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
                objSubGrid.saveRow(rowId,
                                null,
                                null,
                                postInfo,
                                function (id, resp) { //After Save function.
                                    if (resp.responseText.toString().startsWith('Error')) {
                                        //objSubGrid.restoreRow(id, function () { alert('Restore complete'); });
                                        alert(resp.responseText);
                                    }
                                    else {
                                        //Add the row to the parent grid.
                                        var rowData = objSubGrid.getRowData(id);
                                        rowData["Select"] = '&nbsp;<input style="width: 19px;" src="http://localhost/LajitDev1/App_Themes/Lajit/Images/jqGridInfo.png" title="View Details" id="imgSelectRow1" type="image">';
                                        objGrid.addRowData(new Date().getTime(), rowData, 'after', parentRowId);
                                    }

                                    if (rowSubmitCntr == arrRows.length - 1) {

                                        //Delete the original parent grid row now.
                                        DeleteSplitParent(objGrid, parentRowId);

                                        objGrid.trigger('reloadGrid');

                                        //                                        Log('Collapsing the SubGrid');
                                        //                                        objGrid.collapseSubGridRow(parentRowId);
                                        //                                        Log('Resetting the alternating row style to the parent Grid');
                                        //                                        //Reset the Alternating tow style.
                                        //                                        jQuery('tr[role=row]', objGrid).each(function (idx) {
                                        //                                            Log('Applying row style for row : ' + idx);
                                        //                                            Log(this);
                                        //                                            ApplyAlternatingRowStyle(idx, jQuery(this));
                                        //                                        });
                                        //                                        Log('Hiding progress');
                                        //HideProgress();
                                    }

                                    rowSubmitCntr++;
                                },
                                function (id, resp) { //On Error function.
                                    alert('An error has occurred while trying to save row ' + id + '.' + resp.responseText);
                                    if (rowSubmitCntr == arrRows.length - 1) {
                                        HideProgress();
                                    }
                                    rowSubmitCntr++;
                                },
                                null,
                                function (id) {//OnValidation Error.
                                    HideProgress();
                                    noErrors = false;
                                }
                          );
            } //If in Inline Edit end.
            else {
                if (rowSubmitCntr == arrRows.length - 1) {
                    //Last row also has finished submitting.
                    HideProgress();
                }
                rowSubmitCntr++;
            }

        } //For Loop end.

        //If there are no server side posts to be made hide the progress soon after Client-side saving is done.
        if (arrRows.length === 0) {
            HideProgress(); //Uncomment if the above trigger is commented.
        }
        if (noErrors) {
            //        jQuery("#imgbtnInLineSave").hide();
            //        jQuery("#imgbtnInLineCancel").hide();
            //        ShowPager(true);
        }
    } //If IsOk End
    else {
        //Alert the user to correct the imbalance.
        jqAlert(validationErrorStr);
    }
    return false;
}

function CancelSGEdit(rowId) {
    var objGrid = jQuery(gridTableId);
    //Collapse the SubGrid section.
    objGrid.collapseSubGridRow(rowId);
    return false;
}

function SubGridLoadComplete(xhr) {
    //    var objGrid = jQuery(gridTableId);
    //    var objSubGrid = jQuery("#" + subgrid_table_id);
    //    var rowData = objGrid.getRowData(2);
    //    objSubGrid.addRowData(1, rowData, 'last', 1);

}

function OnSubGridSelectRow(rowId, status) {
}

function OnSubGridCollapsed(pID, id) {
    delete g_SubGrid.parentRow;
    return true;
}

function DeleteSplitParent(objGrid, rowIndex) {
    rowIndex = parseInt(rowIndex);
    var nodeRow = GetRow(rowIndex);
    var arrReqVal = GetTrimmedPostInfo(objGrid.getGridParam('userData')); ;
    arrReqVal["selectedRw"] = ConvertToString(nodeRow);
    arrReqVal["oper"] = "del";
    var delUrl = GetUrl();

    jQuery.post(delUrl, arrReqVal, function (data) { Log(data); });
}

//Creates the Op buttons panel for the SubGrid.
function GetBtnsTable(objGrid, objSubGrid, rowId) {
    var btnSave = document.createElement('input');
    btnSave.type = 'image';
    btnSave.style.height = '18px';
    btnSave.onclick = function () { return SaveSGEdit(objGrid, objSubGrid, rowId); };
    btnSave.alt = 'Submit';
    btnSave.src = g_cdnImagesPath + "Images/Modify-but.png";

    var btnCancel = document.createElement('input');
    btnCancel.type = 'image';
    btnCancel.style.height = '18px';
    btnCancel.onclick = function () { return CancelSGEdit(rowId); };
    btnCancel.alt = 'Submit';
    btnCancel.src = g_cdnImagesPath + "Images/Cancel-but.png";

    var tdSave = document.createElement('td');
    tdSave.align = 'center';
    tdSave.appendChild(btnSave);

    var tdCancel = document.createElement('td');
    tdCancel.align = 'center';
    tdCancel.appendChild(btnCancel);

    var tr = document.createElement('tr');
    tr.appendChild(tdSave);
    tr.appendChild(tdCancel);
    var table = document.createElement('table');
    table.cellPadding = "2px";
    table.cellSpacing = "2px";
    table.appendChild(tr);
    var div = document.createElement('div');
    div.width = '100%';
    div.align = 'center';
    div.appendChild(table);
    return div;
}