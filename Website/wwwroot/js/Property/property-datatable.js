$(document).ready(() => {
    console.log("property-datatable called");
    var portfolioId = $("#portfolio_id").val();
    var url = "../api/Properties/GetPropertiesForPortfolio/" + portfolioId;
    var table = $('#portfolioIndexTable').DataTable({
        "processing": true,
        ajax: {
            url: url,
            type: "get",
            dataSrc: ""
        },
        columns: [
            { data: "noOfRooms" },
            { data: "purchaseDate" },
            { data: "propertyValue", render: $.fn.dataTable.render.number(null, null, 2, '&pound;') },
            { data: "monthlyRentAmount", render: $.fn.dataTable.render.number(',', '.', 2, '&pound;') },
            { data: "description" },
            { data: "addressString" },
            { data: "createdDate" },
            { data: "updatedDate" },
            {
                "data": "Actions",
                sortable: false,
                "mRender": function (data, type, row) {
                    return '<a href=/Property/Edit?portfolioId=' + portfolioId + '&propertyId=' + row.id + '><span class="material-icons">edit</span ></a>' + '  ' +
                        '<a href=/Property/Delete?portfolioId=' + portfolioId + '&propertyId='+ row.id + '><span class="material-icons" style="color:red">delete</span ></a>';
                }
            }
        ],
        columnDefs: [
            {
                // Format dates...
                "targets": [1, 6, 7],
                "render": function (data) {
                    if (data === null) {
                        return "-"
                    }
                    return moment(data).format('lll');
                }
            }
        ]
    });

    //// Handle row clicked
    //$('#portfolioIndexTable tbody').on('click', 'tr', function () {
    //    var row = table.row($(this)).data();
    //    //var url = "/Portfolio/Details?id=" + row.id;
    //    //window.location.href = url;
    //});
});