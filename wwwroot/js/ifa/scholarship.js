/**
 SCHOLARSHIP
 */
function GetScholarship(fno, center) {
    $("#divIFAMessage").html("<img src='../images/ajax-loader.gif' />");

    $('#divScholarship').fadeOut();//
    $("#divScholarship").hide();
    $("#divScholarshipContent").html("");

    $('.divAboveSch').fadeOut();//
    $(".divAboveSch").hide();

    $.ajax({
        type: "POST",
        url: '../ifaajax/getScholarship',
        data: { fno: fno, Center: center },
        async: false,
        contentType: "application/x-www-form-urlencoded",
        success: function (data) {
            if (data.scholarship != "") {
                $("#divScholarshipContent").html(data.scholarship);
                $('#divScholarship').fadeIn();//
                $("#divScholarship").show();

                $('.divAboveSch').fadeIn();//
                $(".divAboveSch").show();

                $('#btnprintscholarship').attr('href', '../ifa/scholarship?center=' + center);

                const counters = document.querySelectorAll('.value');
                const speed = 200;

                counters.forEach(counter => {
                    const animate = () => {
                        const value = +counter.getAttribute('akhi');
                        const data = +counter.innerText;

                        const time = value / speed;
                        if (data < value) {
                            counter.innerText = Math.ceil(data + time);
                            setTimeout(animate, value / 4);
                        } else {
                            counter.innerText = value;
                        }

                    }

                    animate();
                });
            }

            $("#divIFAMessage").html("");
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");
            ("#divIFAMessage").html("");
        }
    });
}
