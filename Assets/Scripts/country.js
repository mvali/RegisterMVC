function countryDropdown(seletor){
	var Selected = $(seletor);
	var Drop = $(seletor+'-drop');
	var DropItem = Drop.find('li');

	Drop.find('li').click(function(){
		Selected.removeClass('open');
		Drop.hide();
		
		var item = $(this);
		Selected.html(item.html());
	});

	DropItem.each(function(){
		var code = $(this).attr('data-code');

		if(code != undefined){
			var countryCode = code.toLowerCase();
			$(this).find('i').addClass('flagstrap-'+countryCode);
		}
	});
}
function SelectedShow(countryId) {
    var countryCode = (countryId == 34 ? "CA" : "US");
    var item = $("#country-drop").find("li[data-code='" + countryCode + "']");
    $("#country").html(item.html());
}

countryDropdown('#country');
//$('#ifmap').attr('src', "https://www.google.com/maps/embed/v1/view?key=AIzaSyA4P_jCk3ttS0gVsd5sdDZTqPr-_GBGZG4&center=40.38785,-95.84158&zoom=3");


function ZipChange(zipcodeValue)
{
    var thisObj = $('#ZipCode');
    $("#City").val("");
    $("#State").val("");

    var str = "";
    var zipValid = true;
    var zip = $.trim($(thisObj).val());
    if (zip.length == 0) {
        DisplayDiv(str, "zipLabel", false);
        zipValid = false;
    } else {
        if (!(zip.length >= 5 && zip.length <= 7)) {
            str = "Invalid zip code";
            zipValid = false;
        }
    }
    if (!zipValid) {
        $('#StateRow').hide();
        return;
    }

    var url = "/Controller/Action/";
    var zipFound = 0;
    $.get(url, { zipcode: zip }, function (data) {
        if (Object.keys(data).length > 0) {
            if (data[0].hasOwnProperty("State")) {
                zipFound = 1;
                $("#City").val(data[0].City);
                $("#State").val(data[0].State);
                $("#StateCode").val(data[0].StateCode);
                SelectedShow(data[0].CountryId);
                $("#CountryId").val(data[0].CountryId);
                $('#ifmap').attr('src', "https://www.google.com/maps/embed/v1/place?key=AIzaSyA4P_jCk3ttS0gVsd5sdDZTqPr-_GBGZG4&q=zip:" + zip + "+" + data[0].City + "," + data[0].State);
                //$('#ifmap').attr('src', "https://www.google.com/maps/embed/v1/view?key=AIzaSyA4P_jCk3ttS0gVsd5sdDZTqPr-_GBGZG4&center=" + data[0].Latitude + "," + data[0].Longitude + "&zoom=15");
            }
        } else {
            str = "Zip code not found on our database. Did you write'it correctly?";
        }

        if (zipFound == 1) {
            DisplayDiv(zipFound, "StateRow", true);
            DisplayDiv(zipFound, "zipFound", true);
        } else {
            DisplayDiv(zipFound, "zipNotFound", false);
        }
    });
}

$('#ZipCode').keyup(function () {
    ZipChange($(this).val());
});
$('#ZipCodeBill').keyup(function () {
    ZipChangeBill($(this).val());
});

function DisplayDiv(sValue, inputId, trueforShow) {
    var condition = !trueforShow;
    var inputIdj = "#" + inputId;
    if (sValue.toString().length > 0) {
        if(sValue.toString()!="0")
        condition = !condition;
    }
    $('#zipFound').hide();
    $('#zipInvalid').hide();
    $('#zipNotFound').hide();
    $('#zipLabel').hide();
    $('#StateRow').find('.field-validation-error').each(function (i) { i.empty(); });

    if (condition) {
        $(inputIdj).show();
    } else {
        $(inputIdj).hide();
    }
}

$(document).ready(function () {
    if ($('#ZipCode').val().length > 0) {
        switch ($("#zipaction").val()) {
            case "sv": ZipFromSave(); break;
            case "url": ZipChange(1); break;
            default:;
        }
    }
    if ($('#ZipCodeBill').val().length > 0) {
        switch ($("#zipaction").val()) {
            case "sv": ZipFromSaveBill(); break;
            case "url": ZipChangeBill(1); break;
            default:;
        }
    }
});
function ZipFromSave() {
    var thisObj = $('#ZipCode');
    var zip = $.trim($(thisObj).val());
    var zipValid = true, str="";
    if (zip.length == 0) {
        DisplayDiv(str, "zipLabel", false);
        zipValid = false;
    } else {
        if (!(zip.length == 5 || zip.length == 7)) {
            str = "Invalid zip code";
            DisplayDiv(str, "zipInvalid", true);
            zipValid = false;
        }
    }
    if (!zipValid) {
        $('#StateRow').hide();
        return;
    }
    
    var city = $("#City").val(), state = $("#State").val(), countryId = $("#CountryId").val(), zipFound=0;
    if (city && state && countryId) {
        SelectedShow(countryId); zipFound = 1;
        $('#ifmap').attr('src', "https://www.google.com/maps/embed/v1/place?key=AIzaSyA4P_jCk3ttS0gVsd5sdDZTqPr-_GBGZG4&q=zip:" + zip + "+" + city + "," + state);
    }
    else {
        str = "Zip code not found on our database. Did you write'it correctly ?";
    }

    if (zipFound == 1) {
        DisplayDiv(zipFound, "StateRow", true);
        DisplayDiv(zipFound, "zipFound", true);
    } else {
        DisplayDiv(zipFound, "zipNotFound", false);
    }

}

$('#AddressBillingSame').change(function () {
    BillAddresses($(this).is(":checked"));
});
function BillAddresses(sameAddress) {
    if (sameAddress) {
        $("#addressBill").hide();
        $("#StateRowBill").hide();
    } else {
        $("#addressBill").show();
        ZipFromSaveBill();
    }
}
function ZipFromSaveBill() {
    var thisObj = $('#ZipCodeBill');
    var zip = $.trim($(thisObj).val());
    var zipValid = true, str = "";
    if (zip.length == 0) {
        DisplayDivBill(str, "zipLabelbill", false);
        zipValid = false;
    } else {
        $("#addressBill").show();
        if (!(zip.length == 5 || zip.length == 7)) {
            str = "Invalid zip code";
            //DisplayDivBill(str, "zipInvalidbill", true);
            zipValid = false;
        }
    }
    if (!zipValid) {
        $('#StateRowBill').hide();
        return;
    }

    var countryId = $("#CountryId").val(), zipFound = 0;
    var zipFound = 0;
    if (countryId) {
        SelectedShowBill(countryId); zipFound = 1;
    }
    else {
        str = "Zip code not found on our database. Did you write'it correctly ?";
    }

    if (zipFound == 1) {
        DisplayDivBill(zipFound, "StateRowBill", true);
        DisplayDivBill(zipFound, "zipFoundbill", true);
    } else {
        DisplayDivBill(zipFound, "zipNotFoundbill", false);
    }
}
function DisplayDivBill(sValue, inputId, trueforShow) {
    var condition = !trueforShow;
    var inputIdj = "#" + inputId;
    if (sValue.toString().length > 0) {
        if (sValue.toString() != "0")
            condition = !condition;
    }
    $('#zipFoundbill').hide();
    $('#zipInvalidbill').hide();
    $('#zipNotFoundbill').hide();
    $('#zipLabelbill').hide();
    $('#StateRowBill').find('.field-validation-error').each(function (i) { i.empty(); });

    if (condition) {
        $(inputIdj).show();
    } else {
        $(inputIdj).hide();
    }
}
function SelectedShowBill(countryId) {
    var countryCode = (countryId == 34 ? "CA" : "US");
    var item = $("#country-drop").find("li[data-code='" + countryCode + "']");
    $("#countryBill").html(item.html());
}
function ZipChangeBill(zipcodeValue) {
    var thisObj = $('#ZipCodeBill');
    $("#CityBill").val("");
    $("#StateBill").val("");

    var str = "";
    var zipValid = true;
    var zip = $.trim($(thisObj).val());
    if (zip.length == 0) {
        DisplayDivBill(str, "zipLabelbill", false);
        zipValid = false;
    } else {
        if (!(zip.length == 5 || zip.length == 7)) {
            str = "Invalid zip code";
            DisplayDivBill(str, "zipInvalidbill", true);
            zipValid = false;
        }
    }
    if (!zipValid) {
        $('#StateRowBill').hide();
        return;
    }

    var url = "/Controller/Action/";
    var zipFound = 0;
    $.get(url, { zipcode: zip }, function (data) {
        if (Object.keys(data).length > 0) {
            if (data[0].hasOwnProperty("State")) {
                zipFound = 1;
                $("#CityBill").val(data[0].City);
                $("#StateBill").val(data[0].State);
                $("#StateCodeBill").val(data[0].StateCode);
                SelectedShowBill(data[0].CountryId);
                $("#CountryIdBill").val(data[0].CountryId);
            }
        } else {
            str = "Zip code not found on our database. Did you write'it correctly?";
        }

        if (zipFound == 1) {
            DisplayDivBill(zipFound, "StateRowBill", true);
            DisplayDivBill(zipFound, "zipFoundbill", true);
        } else {
            DisplayDivBill(zipFound, "zipNotFoundbill", false);
        }
    });
}
$(document).ready(function () {
});