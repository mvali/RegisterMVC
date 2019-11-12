function ServiceUpdate(serviceId, add) {
    var changeSuccess = false;
    $.get("/Job/ServiceUpdate", { serviceid: serviceId, opr: add }, function (data) {
        $("span[id='navprice']").each(function () {
            $(this).html(data);
        });
        if (add == 1) { $("#bAdd" + serviceId).hide(); $("#bDel" + serviceId).show(); }
        else { $("#bAdd" + serviceId).show(); $("#bDel" + serviceId).hide(); }
    })
    .done(function () { })
    .fail(function () { })
    .always(function () { })
    ;
}
$(document).ready(function () {
    var go = false;
    if (typeof paid != 'undefined') { if (paid) go = true; }
    if (go) {
        $(".book-page :input").attr("readonly", true);
        $("button").removeAttr('onclick').off('click').on('click', function (e) {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
        });
    } else {
        $('button[id^=bAdd]').click(function () {
            ServiceUpdate($(this).attr('service'), 1);
        });
        $('button[id^=bDel]').click(function () {
            ServiceUpdate($(this).attr('service'), 0);
        });
    }
});
