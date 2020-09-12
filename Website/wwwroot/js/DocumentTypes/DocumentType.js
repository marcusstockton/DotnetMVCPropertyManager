$(document).ready(function () {
    $("#ExpriyDate").parent().hide();

    $("#Expires").on("change", function () {
        if (this.checked) {
            $("#ExpriyDate").parent().show();
        } else {
            $("#ExpriyDate").parent().hide();
        }
    });
});