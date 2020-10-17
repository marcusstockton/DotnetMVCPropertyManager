$(document).ready(function () {

    var table = $('#portfolioIndexTable').DataTable({
        "processing": true,
        ajax: {
            url: "../api/Portfolio/GetMyPortfolios/",
            dataSrc: ''
        },
        "columns": [
            { "data": "name" },
            { "data": "numberOfProperties" },
            { "data": "grossIncome" },
            { "data": "createdDate" },
            { "data": "updatedDate" },
        ],
        columnDefs: [{
            // Format dates...
            targets: [3, 4], render: function (data) {
                return moment(data).format('llll');
            }
        }]
    });

    $('#portfolioIndexTable tbody').on('click', 'tr', function () {
        var loc = window.location;
        var row = table.row($(this)).data();
        var url = "/Portfolio/Details?id=" + row.id; 
        window.location.href = url;

    });
});

