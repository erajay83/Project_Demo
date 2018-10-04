$(document).ready(function () {
    GetUserList();
});

var fixNameFixesList = [];

function GetUserStatistics() {

    var jsonData = {
        
        'UserID': $('#User_ddl').val(),
        'StartDate': $('#dpkDateRangeFrom').val(),
        'EndDate': $('#dpkDateRangeTo').val()
    };
    $.ajax({
        url: "/AdminUserStatistics/GetAdminUserStatics",
        type: "POST",
        data: JSON.stringify(jsonData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (result) {           
            populateGrid(result);
        },
        error: function (httpRequest, textStatus, errorThrown) {
            alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

function populateGrid(result) {
    $("#grid").kendoGrid({
        dataSource: {
            data: result,
            pageSize: 10,
        },
        height: 550,
        filterable: false,
        groupable: false,
        sortable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [{
            field: "User",
            title: "User"
        }, {
            field: "ReportsClosedNewFixAdded",
            title: "Reports Closed<br/>(New Fix Added)"
        }, {
            field: "ReportsClosedExistingFixSelected",
                title: "Reports Closed<br/>(Existing Fix Selected)"
        }, {
            field: "ReportsClosedRejected",
                title: "Reports Closed<br/>(Rejected)"
        }, {
            field: "NumOfFixesFromFixReport",
                title: "# of Fixes from Fix<br/> Report"
        }, {
            field: "NumOfDirectFixes",
            title: "# of Direct Fixes"
        }, {
                field: "NumOfTotalReport",
            title: "Total Reports"
        }]
    });
}

function BindFixNameDropdown() {
    // create DropDownList from input HTML element
    $("#User_ddl").kendoDropDownList({
        dataTextField: "FirstName",
        dataValueField: "AdminUserID",
        dataSource: fixNameFixesList,
        index: 0,
        change: onChange
    });
    var color = $("#User_ddl").data("kendoDropDownList");

    color.select(0);
    function onChange() {

    };
}

function GetUserList() {
    $.ajax({
        url: "/AdminUserStatistics/GetUserDropDown",
        type: "GET",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (result) {
            console.log('result', result);
            $('#loaderImg').hide();
            fixNameFixesList = result;
            BindFixNameDropdown();
        },
        error: function (httpRequest, textStatus, errorThrown) {
            alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

