function pkchoose(pkId) {
    var changeSuccess = false;
    $.get("/Controller/Action", { pkid: pkId }, function (data) {
        $("span[id='navprice']").each(function () {
            $(this).html(data);
        });
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
        }
        $("#bPkAdd" + pkId).hide();
        $("#bPkEx" + pkId).show();
        window.location.href = '/services';
    })
    .done(function () { })
    .fail(function () { })
    .always(function () { });
}
