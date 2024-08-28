/**
 COURSES DETAIL
 */
function GetCourse(fno, center) {
    $("#divIFAMessage").html("<img src='../images/ajax-loader.gif' />");

    $('#divOfflineCourse').fadeOut();//
    $("#divOfflineCourse").hide();
    $("#tableCourse").html("");

    $('#divPhaseTitle').fadeOut();//
    $("#divPhaseTitle").hide();

    $.ajax({
        type: "POST",
        url: '../ifaajax/getCourse',
        data: { fno: fno, Center: center },
        async: false,
        contentType: "application/x-www-form-urlencoded",
        success: function (data) {
            $('#divOfflineCourse').fadeIn();//
            $("#divOfflineCourse").show();

            var courselist = $.parseJSON(data.courselist);

            $("#divPhaseTitle").html(courselist.Table1[0].phasetitle);
            $('#divPhaseTitle').fadeIn();//
            $("#divPhaseTitle").show();        

            var thead = "<thead>";
            thead += "<tr>";
            thead += "<th>Stream</th>";
            thead += "<th>Course Title (Code)</th>";
            thead += "<th>Course Start Date</th>";
            thead += "<th>Phase</th>";
            thead += "<th>Course Fee</th>";
            thead += "</tr>";
            thead += "</thead>";
            var tr = "<tbody>";
            $.each(courselist.Table, function (idx, obj) {
                tr += ('<tr>');
                tr += ('<td>' + obj.streamx + '</td>');
                tr += ('<td>' + obj.coursename + ' (' + obj.course + ')' + '</td>');
                tr += ('<td>' + obj.coursestartdate + '</td>');
                tr += ('<td>' + obj.phase + '</td>');
                tr += ('<td><a href="javascript:void(0)" name="feelink"  id="' + obj.course + '" class="fee btn btn-sm btn-primary">Click here <i class="icon-arrow-right"></i></a></td>');
                tr += ('</tr>');
            });

            tr += "</tbody>";

            $("#tableCourse").html(thead + tr);
            $('#tableCourse').fadeIn();//
            $("#tableCourse").show();

            $("#divIFAMessage").html("");
        },
        error: function (req, status, error) {
            // do something with error
            alert("error");

            $("#divIFAMessage").html("");
        }
    });
}
