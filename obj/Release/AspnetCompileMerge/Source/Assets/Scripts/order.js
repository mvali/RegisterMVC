function reload() {
    window.location.reload();
}
function ispaid() {
    var go = false;
    if (typeof paid != 'undefined') { if (paid) go = true; }
    return go;
}
function disable() {
    if (ispaid()) {
        $(".book-page :input").attr("readonly", true);
        $(".book-page :input[type=checkbox]").attr("disabled", "disabled");
        $("button").removeAttr('onclick').off('click');
        $("form").submit(function (e) {
            alert("Your order was already submited. If you need to modify any data please contact us. Thank You.");
            e.preventDefault();
        });
    }
}
$(document).ready(function () {
    disable();

    var defaultRangeValidator = $.validator.methods.range;
    $.validator.methods.range = function (value, element, param) {
        if (element.type === 'checkbox') {
            // if it's a checkbox return true if it is checked
            return element.checked;
        } else {
            // otherwise run the default validation function
            return defaultRangeValidator.call(this, value, element, param);
        }
    }
});