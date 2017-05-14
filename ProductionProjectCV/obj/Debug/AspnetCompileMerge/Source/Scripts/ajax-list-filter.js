$(document).ready(function () {
    $("#SearchOption_SearchOptionOne").keypress(function (e) {
        if (e.which == 13) {
            $("#cmdSearch").click();
        };
    });
    $("#input-search").keypress(function (e) {
        if (e.which == 13) {
            $("#btn-search").click();
            return;
        };
    });
});

function setTHClass(event) {
    //remove the existing classes
    $(event.currentTarget).parent().find(".sort_wrapper").children("span").removeClass("asc_icon");
    $(event.currentTarget).parent().find(".sort_wrapper").children("span").removeClass("desc_icon");
    if ($("#SearchOption_SearchShortOrder").val() == "ASC") {
        $(event.currentTarget).children(".sort_wrapper").children("span").addClass("asc_icon");
    } else {
        var x = $(event.currentTarget).find("span");
        $(event.currentTarget).find("span").addClass("desc_icon");
    }
}
function SetOrder(event, colName) {
    var oldShortBy = $("#SearchOption_SearchShortBy").val();
    var url = $("#SearchOption_SearchUrl").val();
    $("#SearchOption_SearchShortBy").val(colName);
    if (colName == '') {
        $("#SearchOption_SearchShortOrder").val('');
    }
    else if (oldShortBy == colName) {
        if ($("#SearchOption_SearchShortOrder").val().trim() == '') {
            $("#SearchOption_SearchShortOrder").val('DESC');
        } else {
            $("#SearchOption_SearchShortOrder").val('');
        }
    }
    getHtmlData(url, 'list', 'list_form');
    setTHClass(event);
    //alert(url);
}
function Filter() {
    $("#SearchOption_SearchPageNo").val('1');
    getHtmlData($("#SearchOption_SearchUrl").val(), 'list', 'list_form');
}
function NextPage(pageNo) {
    var url = $("#Page_SearchUrl").val();
    $("#Page_CurrentPageNumber").val(pageNo);
    getHtmlData(url, 'list', 'list_form');

}
function getHtmlData(getUrl, fillDiv, formName) {
    formData = $("form[id$=" + formName + "]").serialize();
    $.ajax(
        {
            type: 'POST',
            url: getUrl,
            data: formData,
            dataType: "HTML",
            async: true,
            cache: false,
            success: function (getData) {
                $("#" + fillDiv).html(getData);

            }
        });

}
function changeRowNo(size) {
    var url = $("#Page.SearchUrl").val();
    $("#Page_CurrentPageNumber").val('1');
    $("#Page_PageSize").val(size);
    getHtmlData(url, 'list', 'list_form');
}
var searchOption = 0;
function RemoveOption(id) {
    //SearchOption += 1;
    $("#search-option" + id).hide();
    $('#SearchOption_SearchParam_' + id + 'SearchFieldName').val("");
    $('#SearchOption_SearchParam_' + id + 'SearchOperator').val("");
    $('#SearchOption_SearchParam_' + id + 'SearchOptionOne').val("");
    $('#SearchOption_SearchParam_' + id + 'SearchOptionTwo').val("");
}
function AddOption(option) {
    if (searchOption == maxOption) {
        return;
    }
    searchOption += 1;
    $("#search-option").append(SearchOptionRow(searchOption,option));

}
var isDataPiker = false;
function SearchOptionRow(i, option) {
    var html = '<div id="search-option' + i + '" class="col-lg-24" >' +
        ' <div class="col-lg-2"> AND ' 
        //<input type="hidden" value="CONTENT" id="SearchOption_SearchParam_' + i + 'SearchFieldName" name="Filter.FilterParam[' + i + '].SearchFieldName">
        +'</div>';
    //'<div  style="overflow: hidden; float: left; width: 90%">';
    var fielsd = option;
    //html += '<div class="col-lg-3"><input type="hidden" id="SearchOption_SearchParam_' + i + 'SearchFieldName" name="Filter.FilterParam[' + i + '].SearchFieldName"></div>';
    html += ' <div class="col-lg-5"><select id="SearchOption_SearchParam_' + i + 'SearchFieldName" name="Filter.FilterParam[' + i + '].SearchFieldName" class="form-control" onchange="fillOperator(' + i + ');"></div>';
    $(fielsd).each(function (index, item) {
        if (item != "") {
            html += '<option value="' + item + '">' + item.split('|')[0] + '</option>';
        }
    });
    html += '</select></div>';
    html += ' <div class="col-lg-3"> ' +
        '<select id="SearchOption_SearchParam_' + i + 'SearchOperator" name="Filter.FilterParam[' + i + '].SearchOperator" onchange="ViewFields(' + i + ');" class="form-control">' +
        '<option value="">--Select--</option>' +
        '</select>' +
        '</div>';
    html += '  <div class="col-lg-5">' +
        '<input type="text"id="SearchOption_SearchParam_' + i + 'SearchOptionOne" name="Filter.FilterParam[' + i + '].SearchOptionOne" class="form-control" /></div>';
    html += '  <div class="col-lg-5" id="Opt' + i + '" style="display:none">' +
        '<input type="text" id="SearchOption_SearchParam_' + i + 'SearchOptionTwo" name="Filter.FilterParam[' + i + '].SearchOptionTwo" class="form-control" /></div>';
    if (i == 0) {
        html += '<div class="col-lg-4"><input type="button" value="Add" onclick="javascript: AddOption();" class="form-control" /></div>';
    } else {
        html += '<div class="col-lg-4"><input type="button" value="Remove" onclick="javascript: RemoveOption(' + i + ');" class="form-control" /></div>';
    }
    html += '</div></div>';
    return html;
}
function ViewFields(id) {
    var dataType = $("#SearchOption_SearchParam_" + id + "SearchOperator").val();
    if (dataType == 'BETWEEN') {
        $("#SearchOptionTwo" + id).show();
        $("#SearchOption_SearchParam_" + id + "SearchOptionTwo").show();
    }
    else {
        $("#SearchOptionTwo" + id).hide();
        $("#SearchOption_SearchParam_" + id + "SearchOptionTwo").hide();
    }
}
function fillOperator(index) {
    if (isDataPiker) {
        $("#SearchOptionOne" + index).html('<input type="text" id="SearchOption_SearchParam_' + index + 'SearchOptionOne" name="Filter.FilterParam[' + index + '].SearchOptionOne" class="form-control" />');
        $("#SearchOptionTwo" + index).html('<input type="text" id="SearchOption_SearchParam_' + index + 'SearchOptionTwo" name="Filter.FilterParam[' + index + '].SearchOptionTwo" class="form-control" />');
    }
    var t = ("#SearchOption_SearchParam_" + index + "SearchFieldName");
    var dataType = $("#SearchOption_SearchParam_" + index + "SearchFieldName").val().split('|')[1];
    if (dataType === 'string') {
        $("#SearchOption_SearchParam_" + index + "SearchOperator").empty();
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="">--Select--</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS">Is</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS-NOT">Is Not</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="BEGIN-WITH">Begins With</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="CONTENT" selected>Contains</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="END-WITH">Ends With</option>');
    } else if (dataType === 'integer') {
        $("#SearchOption_SearchParam_" + index + "SearchOperator").empty();
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="">--Select--</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS" selected>Is</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS-NOT">Is Not</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="BETWEEN">Between</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="GREATER">Greater than</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="LESS">Smaller than</option>');

    }
    else if (dataType == 'DATETIME') {
        $("#SearchOption_SearchParam_" + index + "SearchOperator").empty();
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="">--Select--</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS" selected>Is</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="IS-NOT">Is Not</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="BETWEEN">Between</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="GREATER">Greater than</option>');
        $("#SearchOption_SearchParam_" + index + "SearchOperator").append('<option value="LESS">Smaller than</option>');
        var TempDate = new Date();
        var now = new Date(TempDate.getFullYear() - 10, TempDate.getMonth(), TempDate.getDate() + 1);

        $("#SearchOption_SearchParam_" + index + "SearchOptionTwo").datetimepicker({
            format: dateFormat,
            startDate: now,
            autoclose: true,
            minView: 'month',
            todayHighlight: false
        });
        $("#SearchOption_SearchParam_" + index + "SearchOptionOne").datetimepicker({
            format: dateFormat,
            startDate: now,
            autoclose: true,
            minView: 'month',
            todayHighlight: false
        });
        isDataPiker = true;
    }

}
$('#SearchOption_SearchFieldName').on("change",
    function (event) {
        var dataType = $("#SearchOption_SearchFieldName").val().split('|')[1];
        var x = $("#SearchOption_SearchOperator");
        if (isDataPiker) {
            $("#SearchOptionOne").html('<input type="text"id="SearchOption_SearchOptionOne" name="SearchOption.SearchOptionOne" class="span24" />');
            $("#SearchOptionTwo").html('<input type="text"id="SearchOption_SearchOptionTwo" name="SearchOption.SearchOptionTwo" class="span24" style="display:none"/>');
        }
        if (dataType == 'string') {
            $("#SearchOption_SearchOperator").empty();
            $("#SearchOption_SearchOperator").append('<option value="">--Select--</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS">Is</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS-NOT">Is Not</option>');
            $("#SearchOption_SearchOperator").append('<option value="BEGIN-WITH">Begins With</option>');
            $("#SearchOption_SearchOperator").append('<option value="CONTENT" selected>Contains</option>');
            $("#SearchOption_SearchOperator").append('<option value="END-WITH">Ends With</option>');
        } else if (dataType == 'integer') {
            $("#SearchOption_SearchOperator").empty();
            $("#SearchOption_SearchOperator").append('<option value="">--Select--</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS" selected>IS</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS-NOT">IS NOT</option>');
            $("#SearchOption_SearchOperator").append('<option value="BETWEEN">BETWEEN</option>');
            $("#SearchOption_SearchOperator").append('<option value="GREATER">GREATER-THAN</option>');
            $("#SearchOption_SearchOperator").append('<option value="LESS">LESS-THAN</option>');
        }
        else if (dataType == 'DATETIME') {
            $("#SearchOption_SearchOperator").empty();
            $("#SearchOption_SearchOperator").append('<option value="">--Select--</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS" selected>IS</option>');
            $("#SearchOption_SearchOperator").append('<option value="IS-NOT">IS NOT</option>');
            $("#SearchOption_SearchOperator").append('<option value="BETWEEN">BETWEEN</option>');
            $("#SearchOption_SearchOperator").append('<option value="GREATER">GREATER-THAN</option>');
            $("#SearchOption_SearchOperator").append('<option value="LESS">LESS-THAN</option>');
            $("#SearchOption_SearchOptionTwo").datepicker();
            $("#SearchOption_SearchOptionOne").datepicker();
            isDataPiker = true;
        }
    });

$('#SearchOption_SearchOperator').on("change",
    function (event) {
        var dataType = $("#SearchOption_SearchOperator").val();
        var x = $("#SearchOption_SearchOperator");
        if (dataType == 'BETWEEN') {
            $("#SearchOption_SearchOptionTwo").show();
            $("#SearchOptionTwo").show();
        }
        else {
            $("#SearchOption_SearchOptionTwo").hide();
            $("#SearchOptionTwo").show();
        }
    });