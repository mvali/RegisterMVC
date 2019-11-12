function promocheck() {
    var promo = $("#promoCode").val();
    var pc_appliesTo = 0;
    var pc_discountType = 0;
    var pc_discountValue = 0;
    var pc_msg = "";
    if (promo.length <= 0)
    {
        $("#promomsg").removeClass("text-danger").removeClass("text-success").html("Please enter promocode");
        return;
    }

    $("#promomsg").removeClass("text-danger").removeClass("text-success").html("Verifying. Please wait..");

    $.get("/Payment/PromocodeGet", { promocode: promo }, function (data) {
        if (data.hasOwnProperty("type")) {
            if (data.type >= 0) {
                switch (data.type) {
                    case 1: pc_msg = "Discount: $" + data.value + " USD"; break;
                    case 2: pc_msg = "Discount: " + data.value + "%"; break;
                    default: pc_msg = "Discount: $" + data.value + " USD"; break;
                }
                if (data.appliesTo == 2)
                    pc_msg = pc_msg + " for optional services";

                $("#promomsg").removeClass("text-danger").addClass("text-success").html(pc_msg);
                setTimeout(function () { window.location.reload(true); }, 1500);
            } else {
                switch (data.type) {
                    case -3:
                        pc_msg = "Only for existing customers"; break;
                    case -4:
                        pc_msg = "Sorry, this promo code has expired"; break;
                    case -5:
                        pc_msg = "Sorry, this promo code has expired"; break;
                    default:
                        pc_msg = "Not available";
                }
                $("#promomsg").removeClass("text-success").addClass("text-danger").html(pc_msg);
            }
        }

    })
    .done(function () { })
    .fail(function () { })
    .always(function () { });
}
$(document).ready(function () {
    if (!ispaid()) {
        $('button[id=bPromoCheck]').click(function () {
            promocheck();
        });
    } else {
        $('button[id=bPromoCheck]').click(function () {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
        });
        $("form").submit(function (e) {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
            e.preventDefault();
        });
    }
});
function ispaid() {
    var go = false;
    if (typeof paid != 'undefined') { if (paid) go = true; }
    return go;
}