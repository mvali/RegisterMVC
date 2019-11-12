'use strict';

//var stripe = Stripe('pk_live_lN6eBEgz0TlX0X4TiyyH9NTB');
var stripe = Stripe('pk_test_W2DdRaRLSxfXwLqfnUjsCPOM');
var v_elements;

function registerElements(elements, exampleName) {
  var form = document.forms["Form1"];
  var error = document.querySelector('.errorstripe');
  var errorMessage = error.querySelector('.message');
  v_elements = elements;

  // Listen for errors from each Element, and show error messages in the UI.
  elements.forEach(function(element) {
    element.on('change', function(event) {
      if (event.error) {
        error.classList.add('visible');
        errorMessage.innerText = event.error.message;
      } else {
        error.classList.remove('visible');
      }
    });
  });

  // Listen on the form's 'submit' handler...
  document.getElementById('Form1').addEventListener('submit', function (e) {

      var validated = true;
      var gateMsg = "";
      if (e.preventDefault) e.preventDefault();
      $("#sProcessing").show();

      if (validated) {

          // Gather additional customer data we may have collected in our form.
          var name1 = $("[id$='hjobname']").val();
          var city = $("[id$='hjobcity']").val();
          var state = $("[id$='hjobstate']").val();
          var zip = $("[id$='hjobzip']").val();

          var additionalData = {
              name: name1,
              address_city: city,
              address_state: state,
              address_zip: zip,
          };

          validated = false;
          // Use Stripe.js to create a token. We only need to pass in one Element
          // from the Element group in order to create a token. We can also pass
          // in the additional customer data we collected in our form.
          stripe.createToken(elements[0], additionalData).then(function (result) {
              // Stop loading!
              //example.classList.remove('submitting');

              if (result.token) {
                  // If we received a token, show the token ID.
                  $("[id$='token']").val(result.token.id);
                  // everything ok.. continue submiting
                  validated = true;

                  $('#Form1').unbind('submit');
                  document.forms.Form1.submit();
                  return true;

              } else {
                  // Otherwise, un-disable inputs. //enableInputs();
                  var progress = $("#sProcessing"); if (progress) { progress.hide(); }
                  if (result.error) {
                      error.classList.add('visible');
                      if (result.error.code == "card_declined")
                          errorMessage.innerText = "Your card was declined. Please check your entered credit card details";
                      else
                          errorMessage.innerText = result.error.message;
                  }
              }
          });
      }
      if (!validated) {
          e.preventDefault();
      }
      else {
          $('#Form1').unbind('submit');
          document.forms.Form1.submit();
          return true;
      }
    return validated;
  });

}

