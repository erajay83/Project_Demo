var types = [{
    "TypeId": "0",
    "TypeName": "Action"
},{
    "TypeId": "1",
    "TypeName": "Component"
},{
    "TypeId": "2",
    "TypeName": "Conjunction"
},{
    "TypeId": "3",
    "TypeName": "Qualifier"
}];

function getTypeName(typeId) {
    for (var i = 0, length = types.length; i < length; i++) {
        if (types[i].TypeId === typeId) {
          return types[i].TypeName;
        }
    }
}

$(document).ready(function () {
    $.ajax({
        url: "http://localhost:55337/api/ServiceNameComponents",
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
        },
        error: function (httpRequest, textStatus, errorThrown) {
            alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
        },
        complete: function () {
            $('#loaderImg').hide();
        }
    });

    function typeDropDownEditor(container, options) {
        $('<input data-bind="value:' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "TypeName",
                dataValueField: "TypeId",
                dataSource: types
            });
    }

    function populateGrid(result) {
        $("#grid").kendoGrid({
            dataSource: {
                data: result,
                pageSize: 20,
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Id: { editable: false, nullable: false },
                            TypeId: { field: "TypeId", defaultValue: 0 },
                            Value: { validation: { required: true } }
                        }
                    }
                }
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
            toolbar: ["create"],
            columns: [
                {
                    field: "TypeId",
                    width: "150px",
                    editor: typeDropDownEditor,
                    title: "Type",
                    template: "#=getTypeName(TypeId)#"
                },
                {
                    field: "Value",
                    title: "Value"
                },
                {
                    command: ["edit", "destroy"]
                }
            ],
            editable: "inline"
        });
    }
});
