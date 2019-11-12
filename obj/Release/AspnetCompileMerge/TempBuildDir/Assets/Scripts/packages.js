function pkchoose(pkId) {
    var changeSuccess = false;
    $.get("/Job/PackageUpdate", { pkid: pkId }, function (data) {
        $("span[id='navprice']").each(function () {
            $(this).html(data);
        });
        //if (data == 1) {
        $("button[id^='bPkEx']").each(function () {
            $(this).removeClass("hider");
            $(this).hide();
        });
        $("button[id^='bPkAdd']").each(function () {
            $(this).removeClass("hider");
            $(this).show();
        });
        $("img[id^='selected']").each(function () {
            $(this).removeClass("hider");
            $(this).hide();
        });
        //}
        $("#bPkAdd" + pkId).hide();
        $("#bPkEx" + pkId).show();
        $("#selected" + pkId).show();
    })
    .done(function () { })
    .fail(function () { })
    .always(function () { });
}

$(document).ready(function () {
    var go = false;
    if (typeof paid != 'undefined') { if (paid) go = true; }
    if (go) {
        $(".book-page :input").attr("readonly", true);
        $("button").removeAttr('onclick').off('click').on('click', function (e) {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
        });
    }
});
