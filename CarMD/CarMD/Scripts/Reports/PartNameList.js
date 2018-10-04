$(document).ready(function () {
    GetPartNameList();

    function GetPartNameList() {
        var missingLanguage = $('#missingLanguage').val();
        var keyword = $('#keyword').val();
        var isActive = $('#ActiveCheckbox').is(":checked");
        var isInactive = $('#NotActiveCheckbox').is(":checked");
        var queryString = '';
        if (missingLanguage != '') {
            queryString += 'missingLanguage=' + missingLanguage;
        }
        else {
            if (keyword != '') {
                queryString += 'keyword=' + keyword;
            }
        }
        if (keyword != '' && queryString.indexOf('keyword') == -1) {
            queryString += '&keyword=' + keyword;
        }
        if (queryString == '') {
            queryString += 'isActive=' + isActive + '&isInactive=' + isInactive;
        }
        else {
            queryString += '&isActive=' + isActive + '&isInactive=' + isInactive;
        }
        $.ajax({
            url: "http://localhost:55337/api/PartName/GetPartNameList?" + queryString,
            type: "GET",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            dataType: "json",
            beforeSend: function () {
                $("#loaderImg").show();
            },
            success: function (result) {
                populateGrid(JSON.parse(result));
                $('#loaderImg').hide();
                console.log('JSON.parse(result)', JSON.parse(result));
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
                pageSize: 20,

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
                field: "ACESId",
                title: "ACES ID"
            }, {
                field: "Name",
                title: "Part Name"
            }, {
                field: "Name_es",
                title: "Name ES"
            }, {
                field: "Name_fr",
                title: "Name FR"
            }, {
                field: "Name_zh",
                title: "Name ZH"
                },
                {
                    title: "Act?",
                    width: 240,
                    template:  "#if(IsActive==true){#Yes#}else{#No #}#"
                },
                {
                    title: "Active",
                    template: '<a href="\\#" onclick="GetFixesInfo(#=PartNameId#)" >Edit</a> | <a href="\\#" onclick="DeletePartName(#=PartNameId#)" >Delete</a> '
                }
                
            ]
        });
    }
    $('#search').click(function () {
        GetPartNameList();
    });
    // Delete part name 
    function DeletePartName(id) {
        alert(id);
    }
    //Old Code for fetching data from DB
    //$.ajax({
    //    url: "http://localhost:55337/api/PendingFixes",
    //    type: "GET",
    //    headers: {
    //        "Content-Type": "application/x-www-form-urlencoded"
    //    },
    //    dataType: "json",
    //    beforeSend: function () {
    //        $("#loaderImg").show();
    //    },
    //    success: function (result) {
    //        populateGrid(JSON.parse(result));
    //        console.log('JSON.parse(result)', JSON.parse(result));
    //    },
    //    error: function (httpRequest, textStatus, errorThrown) {
    //        alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
    //    },
    //    complete: function () {
    //        $('#loaderImg').hide();
    //    }
    //});
});
