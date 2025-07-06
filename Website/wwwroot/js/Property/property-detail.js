let postcodeMap = null;

$(() => {
    $("#error-alert").hide();

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

    // Listen for the tab activation event
    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        if ($(e.target).attr('href') === '#nav-home') {
            initPostcodeMap();
            if (postcodeMap) {
                setTimeout(() => postcodeMap.invalidateSize(), 200); // Ensure correct sizing
            }
        }
    });

    // initialize if the tab is already active on page load
    if ($('#nav-home').hasClass('active')) {
        initPostcodeMap();
        if (postcodeMap) {
            setTimeout(() => postcodeMap.invalidateSize(), 200);
        }
    }

    function initPostcodeMap() {
        if (postcodeMap) return; // Prevent multiple initializations

        var addressLat = $('#Address_Latitude').val();
        var addressLon = $('#Address_Longitude').val();
        if (addressLat && addressLon && addressLat !== '0.000000000' && addressLon !== '0.000000000') {
            console.log($('#Address_Latitude').val(), $('#Address_Longitude').val());
            postcodeMap = L.map('postcodeImg').setView([addressLat, addressLon], 16);
            L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            }).addTo(postcodeMap);
            L.marker([addressLat, addressLon]).addTo(postcodeMap);
        }
    }
});