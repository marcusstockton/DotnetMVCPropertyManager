$(document).ready(function () {
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

            }, error: function (err) {
                console.error(err.error);
            }
        });
    });
});