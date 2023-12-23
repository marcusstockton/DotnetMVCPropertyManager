$(document).ready(function () {
    console.log("createTenantModal.js loaded!");
    $('#tenantCreateButton').click(function () {
        var portfolioId = $('#portfolio_id').val();
        var propertyId = $('#property_id').val();
        var url = $('#tenantCreateButton').data('url');

        $.ajax({
            url: url,
            type: "get", //send it through get method
            data: {
                portfolioId: portfolioId,
                propertyId: propertyId,
            },
            success: function (response) {
                //Do Something
                $('#configureBody').html(response);
                $('#tenantCreateModal').modal('hide');
            },
            error: function (ex) {
                //Do Something to handle error
                alert(ex);
            }
        });
    });

    $('#tenantCreateSave').on("click", function () {
        var portfolioId = $('#portfolio_id').val();
        var propertyId = $('#property_id').val();

        var form = $('#tenantCreateForm')[0];
        var formData = new FormData(form);
        formData.append('portfolioId', portfolioId);
        formData.append('propertyId', propertyId);

        $.ajax({
            async: true,
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            url: "/Tenants/Create",
            success: function (result) {
                console.log(result);
                $('#tenantCreateModal').modal('hide');
                location.reload();
            }, error: function (err) {
                console.error(err.error);
            }
        });
    });

    $('#tenantCreateModal').on('keyup', '#JobTitle', function (event) {
        var target = $(this);
        var url = "../api/Tenant/job-title-autocomplete"
        target.autocomplete({
            delay: 500,
            appendTo: "#tenantCreateForm",
            source: function (request, response) {
                $.ajax({
                    url: url,
                    dataType: "json",
                    data: {
                        jobTitle: request.term
                    },
                    type: "GET",
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.replace(/(^\w{1})|(\s+\w{1})/g, letter => letter.toUpperCase()), // capitalises first letter of each word
                                value: item.replace(/(^\w{1})|(\s+\w{1})/g, letter => letter.toUpperCase())
                            };
                        }));
                    }
                });
            },
            minLength: 2,
            open: function () {
                $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
            },
            close: function () {
                $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
            }
        });
    });
});