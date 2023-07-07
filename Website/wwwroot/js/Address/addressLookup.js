$(document).ready(function () {
    console.log("addressLookup.js loaded");

    $('#enableautocomplete').change(function () {
        if (this.checked) {
            addressAutocomplete();
        }
    });

    
        
    
    

//    $('#Address_Postcode').autocomplete({
///*        delay: 500,*/
//        minLength: 3,
//        source: function (request, response) {
//            $.ajax({
//                url: "../api/address/postcode-auto-complete",
//                dataType: "json",
//                data: {
//                    postcode: request.term
//                },
//                type: "GET",
//                success: function (data) {
//                    response($.map(data, function (item) {
//                        return {
//                            label: item,
//                            value: item
//                        };
//                    }));
//                }
//            });
//        },
//        select: function (event, ui) {
//            $.get("../api/address/postcode-lookup", { postcode: ui.item.value }, function (data, status) {
//                //$("#Address_Line1").val(data.address.houseNumber);
//                //$("#Address_Line2").val(data.address.street);
//                $("#Address_Line3").val(data.result.parish);
//                $("#Address_City").val(data.result.admin_district);
//                //$("#Address_Postcode").val(data.address.postalCode);
//                $("#Address_Latitude").val(data.result.latitude);
//                $("#Address_Longitude").val(data.result.longitude);
//            });
//        },
//    });
});

function addressAutocomplete() {
    $("#Address_Line1").autocomplete({
        delay: 500,
        source: function(request, response) {
            $.ajax({
                url: "../api/address/GetAutoSuggestion",
                dataType: "json",
                data: {
                    search: request.term
                },
                type: "GET",
                success: function(data) {
                    response($.map(data.items, function(item) {
                        return {
                            label: item.title,
                            value: item.id
                        };
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            $.get("../api/address/Lookup", { hereId: ui.item.value }, function(data, status) {
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
        open: function() {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function() {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }
    });
}
