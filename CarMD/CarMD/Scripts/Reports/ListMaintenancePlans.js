$(document).ready(function () {

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
    //    },
    //    error: function (httpRequest, textStatus, errorThrown) {
    //        alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
    //    },
    //    complete: function () {
    //        $('#loaderImg').hide();
    //    }
    //});


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
                field: "FirstName",
                title: "First Name"
            }, {
                field: "LastName",
                title: "Last Name"
            }, {
                field: "EmailAddress",
                title: "Email Address"
            }]
        });
    }

    


});



