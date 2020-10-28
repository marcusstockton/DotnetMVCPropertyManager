$(document).ready(function () {

    var table = $('#portfolioIndexTable').DataTable({
        //"processing": true,
        ajax: {
            url: "../api/Portfolio/GetMyPortfolios/",
            dataSrc: '',
            cache: true,
        },
        columns: [
            { "data": "name" },
            { "data": "numberOfProperties" },
            { "data": "totalPropertyValue", render: $.fn.dataTable.render.number(',', '.', 2, '£') },
            { "data": "grossIncome", render: $.fn.dataTable.render.number(',', '.', 2, '£') },
            { "data": "createdDate" },
            { "data": "updatedDate" },
            {
                "data": "Actions",
                sortable: false,
                "mRender": function (data, type, row) {
                    return '<a href=/Portfolio/Edit?id=' + row.id + '><span class="material-icons">edit</span ></a>' + '  ' + 
                        '<a href=/Portfolio/Delete?id=' + row.id + '><span class="material-icons" style="color:red">delete</span ></a>';
                }
            }
        ],
        columnDefs: [
            {
                "targets": 1,
                "className": "text-center",
            },
            {
                // Format dates...
                "targets": [4, 5],
                "render": function (data) {
                    if (data === null) {
                        return "-"
                    }
                    return moment(data).format('llll');
                }
            }
        ]
    });

    // Handle row clicked
    $('#portfolioIndexTable tbody').on('click', 'tr', function () {
        var row = table.row($(this)).data();
        var url = "/Portfolio/Details?id=" + row.id; 
        window.location.href = url;
    });

});

