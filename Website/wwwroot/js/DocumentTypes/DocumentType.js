$(document).ready(function () {
    $("#ExpiryDate").parent().hide();

    $("#Expires").on("change", function () {
        if (this.checked) {
            $("#ExpiryDate").parent().show();
        } else {
            $("#ExpiryDate").parent().hide();
        }
    });
});