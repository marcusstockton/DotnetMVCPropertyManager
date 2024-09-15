$(function () {
    console.log("addressLookup.js loaded");

    $('#enableautocomplete').change(function () {
        if (this.checked) {
            addressAutocomplete();
        }
    });
});

function addressAutocomplete() {
    var cache = {};
    $("#Address_Line1").autocomplete({
        delay: 500,
        source: function (request, response) {
            var term = request.term;
            if (term in cache) {
                response(cache[term]);
                return;
            }
            $.getJSON({
                url: "../api/address/GetAutoSuggestion",
                dataType: "json",
                data: {
                    search: term
                },
                type: "GET",
                success: function (data) {
                    cache[term] = data;
                    response($.map(data.items, function (item) {
                        return {
                            label: item.title,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 5,
        select: function (event, ui) {
            console.log(`Item Selected: ${ui.item}`)
            $.get("../api/address/Lookup", { hereId: ui.item.value }, function (data, status) {
                var data = JSON.parse(data);
                $("#Address_Line1").val(data.address.houseNumber);
                $("#Address_Line2").val(data.address.street);
                $("#Address_Town").val(data.address.district);
                $("#Address_City").val(data.address.city);
                $("#Address_Postcode").val(data.address.postalCode);
                $("#Address_Latitude").val(data.position.lat);
                $("#Address_Longitude").val(data.position.lng);
            });
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
}