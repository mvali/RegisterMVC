function ServiceUpdate(serviceId, add) {
    var changeSuccess = false;
    $.get("/Controller/Action", { serviceid: serviceId, opr: add }, function (data) {
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
});
