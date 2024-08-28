/**
 LOGOUT SESSION
 */
function logout() {
    $.ajax({
        type: "POST",
        url: '../logout/',
        async: false,
        contentType: "application/x-www-form-urlencoded",
        success: function (data) {
            window.location = "../home/index";
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");
        }
    });
}
