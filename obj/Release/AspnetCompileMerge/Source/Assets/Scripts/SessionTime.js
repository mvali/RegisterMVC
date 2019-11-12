﻿$(document).ready(function () {
    $('#tooltipa').tooltip({
        content: $('#tooltip_box'), // set the content to be the 'blah' div
        placement: 'bottom',
        html: true
    });
    // The popover contents will not be set until the popover is shown.  Since we don't 
    // want to see the popover when the page loads, we will show it then hide it.
    //
    $('#tooltipa').tooltip('show');
    $('#tooltipa').tooltip('hide');

    $("#tooltipa").mouseenter(function () {
        $("#tooltip_box").show();
    });
    $("#tooltipa").mouseleave(function () {
        $("#tooltip_box").hide();
    });
    disable();
});
function CheckAvailabilityTd() {
    if (chosendate == "0") {
        $("#notavailabled").show();
    } else {
        $("#notavailabled").hide();
    }
}
function CheckAvailabilityT(testime) {
    if (chosendate == "1") {
        if (testime < ds || testime > de) {
            $("#notavailablet").show();
        } else {
            $("#notavailablet").hide();
        }
    }
}
/*begin time section*/
function disable(){
    if (!ispaid()) {
        $(".photo-take").find("li").click(function () {
            DeactivateH();
            var item = $(this);
            item.addClass("active");
            $("#PhotoSessionTime").val(item.attr("hr"));
            CheckAvailabilityT(item.attr("hr"));
        })
        CheckAvailabilityTd();
    } else {
        $(".photo-take").find("li").click(function () {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
        });
        $("form").submit(function (e) {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
            e.preventDefault();
        });
    }
}
function DeactivateH() {
    $(".photo-take").find("li.active").removeClass("active");
}
/*end time section*/

function ispaid() {
    var go = false;
    if (typeof paid != 'undefined') { if (paid) go = true; }
    return go;
}