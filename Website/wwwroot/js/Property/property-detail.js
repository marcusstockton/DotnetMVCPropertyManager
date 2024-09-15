$(() => {
    let url = location.href.replace(/\/$/, "");

    // Handles opening correct tab if the url contains the hash of a tab
    if (location.hash) {
        const hash = url.split("#");
        var tabSelect = $('#nav-tab button[href="#' + hash[1] + '"]');
        tabSelect.tab("show")
        url = location.href.replace(/\/#/, "#");
        history.replaceState(null, null, url);
        setTimeout(() => {
            $(window).scrollTop(0);
        }, 400);
    }

    $('a[data-bs-toggle="tab"]').on("click", function () {
        let newUrl;
        const hash = $(this).attr("href");
        if (hash == "#nav-home") {
            newUrl = url.split("#")[0];
        } else {
            newUrl = url.split("#")[0] + hash;
        }
        newUrl += "/";
        history.replaceState(null, null, newUrl);
    });

    var addressLat = $('#Address_Latitude').val();
    var addressLon = $('#Address_Longitude').val();
    var propertyId = $('#property_id').val();
    var portfolioId = $('#portfolio_id').val();
    if (addressLat != '0.000000000' && addressLon != '0.000000000') {
        console.log(`Calling GetMapFromLatLong with PortfolioID: ${portfolioId}, PropertyID: ${propertyId}, Lat: ${addressLat}, Lon: ${addressLon}`);
        $.ajax({
            url: "../api/address/GetMapFromLatLong",
            data: {
                portfolioId: portfolioId,
                propertyId: propertyId,
                lat: addressLat,
                lon: addressLon
            },
            cache: false,
            type: "GET",
            success: function (response) {
                $('#postcodeImg').attr("src", "data:" + response).removeAttr('hidden');
            },
            error: function (xhr) {
                console.log(xhr);
            }
        });
    }
});