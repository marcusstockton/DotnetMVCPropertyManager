$(function () {
    $('.tenantDetailsButton').on("click", function (event) {
        var url = $(this).closest("button").data("url");
        $.ajax({
            url: url,
            type: "get",
            success: function (response) {
                $('#configureBody').html(response);
            },
            error: function (ex) {
                alert(ex);
            }
        });
    });
});