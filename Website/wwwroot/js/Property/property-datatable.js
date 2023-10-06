$(document).ready(() => {
    console.log("property-datatable called");
    var portfolioId = $("#portfolio_id").val();
    console.log("PortfolioId: " + portfolioId);
    var url = "../../api/Properties/GetPropertiesForPortfolio/" + portfolioId;
    var table = $('#portfolioIndexTable').DataTable({
        "processing": true,
        ajax: {
            url: url,
            type: "get",
            dataSrc: ""
        },
        columns: [
            { data: "noOfRooms" },
            { data: "purchaseDate", render: DataTable.render.datetime('DD/MM/YYYY') },
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
            }, {
                "targets": [4, 5],
                "render": function (data, type, row) {
                    if (data.length > 60) {
                        return data.substr(0, 60) + "...";
                    } else {
                        return data;
                    }
                    
                }
            }
        ]
    });

    // Handle row clicked
    $('#portfolioIndexTable tbody').on('click', 'tr', function () {
        var row = table.row($(this)).data();
        //var portfolioId = $('#portfolio_id').val();
        //https://localhost:44342/Property/Detail?portfolioId=3ae9e10c-2e31-4766-6152-08db7ed9c2c8&propertyId=bcbf8585-6959-481d-dea2-08db7ed9c2ca
        var url = "/Property/Detail?portfolioId=" + portfolioId + "&propertyId=" + row.id;
        window.location.href = url;
    });
});