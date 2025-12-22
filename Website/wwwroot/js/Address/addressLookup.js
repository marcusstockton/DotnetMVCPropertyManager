$(function () {
    console.log("addressLookup.js loaded");

    $(function () {
        const $input = $('#Address_Postcode');
        if (!$input.length) return;

        $input.autocomplete({
            source(request, response) {
                $.getJSON('/api/Address/postcode-auto-complete', { postcode: request.term })
                    .done(data => response(data || []))
                    .fail(() => response([]));
            },
            minLength: 2,
            delay: 250,
            select(event, ui) {
                const val = ui.item.value ?? ui.item.label;
                $input.val(val).trigger('change');

                // Call postcode-lookup and update latitude / longitude hidden inputs
                $.get('/api/Address/postcode-lookup', { postcode: val })
                    .done(function (data) {
                        try {
                            let obj = data;
                            // controller may return a JSON string — try to parse if needed
                            if (typeof data === 'string') {
                                obj = JSON.parse(data);
                            }
                            // response shape: { status:200, result: { latitude: .., longitude: .. } }
                            const res = obj.result ?? obj;
                            if (res) {
                                const lat = res.latitude ?? res.lat ?? res.Latitude;
                                const lng = res.longitude ?? res.lng ?? res.Longitude;
                                if (lat !== undefined) {
                                    $('#Address_Latitude').val(lat).trigger('change');
                                }
                                if (lng !== undefined) {
                                    $('#Address_Longitude').val(lng).trigger('change');
                                }
                            }
                        } catch (err) {
                            console.error('postcode lookup parse error', err, data);
                        }
                    })
                    .fail(function () {
                        console.warn('postcode lookup failed');
                    });

                return false;
            }
        }).autocomplete('instance')._renderItem = function (ul, item) {
            return $('<li>')
                .addClass('list-group-item list-group-item-action')
                .append($('<div>').text(item.label ?? item.value))
                .appendTo(ul);
        };

        // ensure menu is above other UI elements
        $('<style>.ui-autocomplete{z-index:10000}</style>').appendTo('head');
    });

});