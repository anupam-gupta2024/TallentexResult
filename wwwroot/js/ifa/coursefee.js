$(document).on('click', '.fee', function () {
    var fno = $("#Fno").val();
    var center = $("#Center").val();
    var course = this.id;

    getCourseFee(fno, center, course);
});
/**
 COURSES FEE DETAIL
 */
function getCourseFee(fno, center, course) {
    $('#divOfflineCourseFee').fadeOut();//
    $("#divOfflineCourseFee").hide();

    $("#tableCourseFee").html("");
    $("#divCourseFeeHeader").html("");

    $.ajax({
        type: "POST",
        url: '../ifaajax/getCourseFee',
        data: { fno: fno, Center: center, Course: course },
        async: false,
        contentType: "application/x-www-form-urlencoded",
        success: function (data) {
            $('#divOfflineCourseFee').fadeIn();//
            $("#divOfflineCourseFee").show();

            var coursefeelist = $.parseJSON(data.coursefeelist);

            $("#divCourseFeeHeader").html(coursefeelist.Table1[0].Course);
            $('#divCourseFeeHeader').fadeIn();//
            $("#divCourseFeeHeader").show();


            var tbody = "<tbody>";
            var tr = "";
            var colspan = 0;
            var isamt1 = isamt2 = isamt3 = istotalamt = isamt = false;
            $.each(coursefeelist.Table, function (idx, obj) {
                var thead = "";

                if (obj.PlanTitle == "") {
                    if (obj.Amount1 != "") { isamt1 = true; colspan++; }
                    if (obj.Amount2 != "") { isamt2 = true; colspan++; }
                    if (obj.Amount3 != "") { isamt3 = true; colspan++; }
                    if (obj.TotalAmount != "") { istotalamt = true; colspan++; }
                    if (obj.Amount != "") { isamt = true; colspan++; }
                }
                else {
                    thead += ('<tr><th colspan="' + colspan + '">' + obj.PlanTitle + '</th></tr>');
                }

                var th = "<tr>"; var td = "<tr>";
                if (isamt1) {
                    th += ('<th>I-Installment</th>');
                    td += ('<td>' + obj.Amount1 + '</td>');
                }
                if (isamt2) {
                    th += ('<th>II-Installment</th>');
                    td += ('<td>' + obj.Amount2 + '</td>');
                }
                if (isamt3) {
                    th += ('<th>III-Installment</th>');
                    td += ('<td>' + obj.Amount3 + '</td>');
                }

                if (istotalamt) {
                    th += ('<th>Installment Total</th>');
                    td += ('<td>' + obj.TotalAmount + '</td>');
                }

                if (isamt) {
                    th += ('<th>If paid in Lumpsum</th>');
                    td += ('<td>' + obj.Amount + '</td>');
                }

                th += "</tr>"; td += "</tr>";

                tbody += thead + th + td;
            });

            tbody += "</tbody>";

            $("#tableCourseFee").html(tbody);
            $('#tableCourseFee').fadeIn();//
            $("#tableCourseFee").show();

            var scrollPos = $("#tableCourseFee").offset().top;
            $(window).scrollTop(scrollPos);
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");
        }
    });
}
