$(document).ready(function () {
    GetMakeList();
    BindModelDropdown();
    BindYearDropdown();

    $("#btnRunReport").click(function () {
        //GetRecallsReport();
    });
});


// Bind Make dropdown
function GetMakeList() {
    $.ajax({
        url: "/PredictiveDiagnosticReport/GetMakeList",
        type: "GET",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (result) {

            MakeList = result;
            BindMakeDropdown();
        },
        error: function (httpRequest, textStatus, errorThrown) {

        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

function BindMakeDropdown() {
    // create DropDownList from input HTML element
    $("#MakeList_ddl").kendoDropDownList({
        dataTextField: "Value",
        dataValueField: "Value",
        dataSource: MakeList,
        index: 0,
        change: onChangeMake
    });
    var color = $("#MakeList_ddl").data("kendoDropDownList");

    color.select(0);
    function onChangeMake() {
        GetModelList();
    };
}

// Bind Model dropdown
function GetModelList() {
    var jsonData = {
        'Make': $('#MakeList_ddl').val()
    };
    $.ajax({
        type: "POST",
        url: "/PredictiveDiagnosticReport/GetModelListByMake",
        data: JSON.stringify({ 'model': jsonData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (response) {
            ModelList = response;
            BindModelDropdown();
        },
        failure: function (response) {
        },
        error: function (response) {

        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

function BindModelDropdown() {
    // create DropDownList from input HTML element
    $("#ModelList_ddl").kendoDropDownList({
        dataTextField: "CleanModel",
        dataValueField: "CleanModel",
        dataSource: ModelList,
        index: 0,
        change: onChangeMake
    });
    var color = $("#ModelList_ddl").data("kendoDropDownList");

    color.select(0);
    function onChangeMake() {
        GetYearList();
    };
}

// Bind Year dropdown
function GetYearList() {
    var jsonData = {
        'Make': $('#MakeList_ddl').val()
    };
    $.ajax({
        type: "POST",
        url: "/PredictiveDiagnosticReport/GetYearListByMake",
        data: JSON.stringify({ 'model': jsonData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (response) {
            // Added "No Selection" on first index
            if (response.length > 0) {
                response[0].Year = "No Selection";
            }
            YearList = response;
            BindYearDropdown();
        },
        failure: function (response) {
        },
        error: function (response) {

        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

function BindYearDropdown() {
    // create DropDownList from input HTML element
    $("#YearList_ddl").kendoDropDownList({
        dataTextField: "Year",
        dataValueField: "Year",
        dataSource: YearList,
        index: 0
        //change: onChangeMake
    });
    var color = $("#YearList_ddl").data("kendoDropDownList");
    color.select(0);
}

// Bind Year dropdown
function GetEngineList() {
    var jsonData = {
        'Make': $('#EngineList_ddl').val()
    };
    $.ajax({
        type: "POST",
        url: "/RecallsReport/GetYearListByMake",
        data: JSON.stringify({ 'model': jsonData }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        beforeSend: function () {
            $("#loaderImg").show();
        },
        success: function (response) {
            // Added "No Selection" on first index
            if (response.length > 0) {
                response[0].Year = "No Selection";
            }
            YearList = response;
            BindYearDropdown();
        },
        failure: function (response) {
        },
        error: function (response) {

        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });
}

function BindEngineDropdown() {
    // create DropDownList from input HTML element
    $("#EngineList_ddl").kendoDropDownList({
        dataTextField: "Year",
        dataValueField: "Year",
        dataSource: YearList,
        index: 0
        //change: onChangeMake
    });
    var color = $("#EngineList_ddl").data("kendoDropDownList");
    color.select(0);
}