$(function () {
    $("[name='profilePic']").on("change", function (ele) {
        readfile(ele.currentTarget)
    });

    function readfile(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('.img-thumbnail').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
            $('#preview').html("This is a preview of your new image").css("font-weight", "Bold");;
        }
    }
});