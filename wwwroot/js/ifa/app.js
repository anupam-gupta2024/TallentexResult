$('[name="Mode"]').on('change', function (event) {
    $("#Mode").css("cursor", "wait");
    $("#Mode").prop('disabled', true);

    /*$('#divScholarship').fadeOut();*/
    $("#divScholarship").hide();
    $("#divScholarshipContent").html("");

    $('#divOnlineCourse').fadeOut();//
    $("#divOnlineCourse").hide();

    $('#divOfflineCourse').fadeOut();//
    $("#divOfflineCourse").hide();

    $('#divOfflineCourseFee').fadeOut();//
    $("#divOfflineCourseFee").hide();

    $('.divAboveSch').fadeOut();//
    $(".divAboveSch").hide();

    $("#Center").val("");
    $('#divCenterSelect').fadeOut();//
    $("#divCenterSelect").hide();

    var mode = this.value;

    if (mode == "1") {
        $('#divCenterSelect').fadeIn();//
        $('#divCenterSelect').show();//        
    }
    else if (mode == "2") {
        GetScholarship($("#Fno").val(), "9610");

        $('#divOnlineCourse').fadeIn();//
        $("#divOnlineCourse").show();
    }

    $('#Mode').prop('disabled', false);
    $("#Mode").css("cursor", "pointer");
});


$('[name="Center"]').on('change', function (event) {    
    $("#Center").css("cursor", "wait");
    $("#Center").prop('disabled', true);

    var center = this.value;
    var fno = $("#Fno").val();
    var classs = $("#Class").val();

    $('.CommerceInterested').hide();//

    if (center != "") {
        GetScholarship(fno, center);
        GetCourse(fno, center);        
        CommerceInterested(center, classs);
    }

    $('#Center').prop('disabled', false);
    $("#Center").css("cursor", "pointer");
});

function CommerceInterested(center, classs) {
    $('.CommerceInterested').hide();//

    if ((classs == "10") && (center == "1101" || center == "4001" || center == "1001")) {
        $('.CommerceInterested').show();//
    }
}