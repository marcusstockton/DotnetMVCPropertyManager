﻿$(function () {
    var table = $('#portfolioIndexTable').DataTable({
        processing: true,
        //serverSide: true,
        ajax: {
            url: "../api/Portfolio/GetMyPortfolios/",
            dataSrc: '',
            type: "GET",
            cache: false,
            headers: {
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache',
                'Expires': '0'
            }
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
                    return '<a href=/Portfolio/Edit/' + row.id + '><span class="material-icons">edit</span ></a>' + '  ' +
                        '<a href=/Portfolio/Delete/' + row.id + '><span class="material-icons" style="color:red">delete</span ></a>';
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
                    return dayjs(data).format('ddd, MMM D, YYYY HH:mm');
                }
            }
        ]
    });

    // Handle row clicked
    $('#portfolioIndexTable tbody').on('click', 'tr', function () {
        var row = table.row($(this)).data();
        var url = "/Portfolio/Details/" + row.id;
        window.location.href = url;
    });
});