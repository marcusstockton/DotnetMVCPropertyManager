$(document).ready(function () {
    console.log("jobTitleAutocomplete.js loaded!");
    $('#JobTitle').autocomplete({
        delay: 500,
        appendTo: "#tenantCreateForm",
        source: function (request, response) {
            $.ajax({
                url: "../api/Tenant/job-title-autocomplete",
                dataType: "json",
                data: {
                    jobTitle: request.term
                },
                type: "GET",
                success: function (data) {
                    response($.map(data.items, function (item) {
                        return {
                            label: item.title,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 2,
        //select: function (event, ui) {
        //    $.get("../api/address/Lookup", { hereId: ui.item.value }, function (data, status) {
        //        var data = JSON.parse(data);
        //        $("#Address_Line1").val(data.address.houseNumber);
        //        $("#Address_Line2").val(data.address.street);
        //        $("#Address_Line3").val(data.address.district);
        //        $("#Address_City").val(data.address.city);
        //        $("#Address_Postcode").val(data.address.postalCode);
        //        $("#Address_Latitude").val(data.position.lat);
        //        $("#Address_Longitude").val(data.position.lng);
        //    });
        //},
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
});