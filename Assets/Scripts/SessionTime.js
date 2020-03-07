$(document).ready(function () {
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
        if(ph>0)$("#notavailabled").show();
    } else {
        $("#notavailabled").hide();
    }
}
function CheckAvailabilityT(testime) {
    if (chosendate == "1") {
        if (+testime < +ds || +testime > +de) {
            $("#notavailablet").show();
        } else {
            $("#notavailablet").hide();
        }
    }
}
/*begin time section*/
function disable(){
        $(".photo-take").find("li").click(function () {
            DeactivateH();
            var item = $(this);
            item.addClass("active");
            $("#PhotoSessionTime").val(item.attr("hr"));
            CheckAvailabilityT(item.attr("hr"));
        })
        CheckAvailabilityTd();
}
function DeactivateH() {
    $(".photo-take").find("li.active").removeClass("active");
}/*end time section*/
