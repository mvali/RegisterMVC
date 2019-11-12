using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    [JobControllerFilter(DestControler = "Order", DestAction = "Index")]
    public class PaymentController : Controller
    {
        // GET: Payment
        [HttpGet]
        public ActionResult Index()
        {
            var j = Models.Job.Session();
            var sl = new List<Models.Service>();
            var p = new Models.Package();
            decimal promoCodeAmount = 0;

            Decimal totalAmount = Models.Job.AmountTotal(true, ref sl, ref p, ref promoCodeAmount);
            ViewBag.totalAmount = totalAmount;
            ViewBag.servicesList = sl;
            ViewBag.package = p;
            ViewBag.job = j;
            ViewBag.promoDiscount = promoCodeAmount;
            ViewBag.promo = j.Promocode == null ? "" : j.Promocode.name;

            return View("Payment",new Models.Payment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.Payment payd)
        {
            var j = Models.Job.Session();
            if (j.Paid)
            {
                ViewBag.msg = -10;
                return View("Payment", payd);
            }

            string errorCode = "";

            Random rnd = new Random();
            int invoice = rnd.Next(10000, 99999);

            System.Collections.Specialized.NameValueCollection TranArray = new System.Collections.Specialized.NameValueCollection();
            TranArray.Add("email", j.JobPersonalInfo.JobEmail);
            TranArray.Add("firstname", j.JobPersonalInfo.FirstName);
            TranArray.Add("lastname", j.JobPersonalInfo.LastName);
            TranArray.Add("invoice", invoice.ToString());
            TranArray.Add("amount", Utils.MathDecimals(Models.Job.AmountTotal()));// to update with canadian value
            TranArray.Add("amountusd", Utils.MathDecimals(Models.Job.AmountTotal()));
            TranArray.Add("token", payd.token);

            bool bPayment = false;
            Models.JsonStripeResponse jsRet = new Models.JsonStripeResponse();
            //if (ValidCC(ref errorCode))
            if (payd.token.Length > 5)
            {
                bPayment = Models.Payment.PayProcessStripe(j.Id, TranArray, ref errorCode, ref jsRet);
            }
            else
            {
                ViewBag.msg = -2;
                return View("Payment", payd);
                //errorCode = "errorCode"; //given by validate function
                //bPay.Disabled = false;
            }
            if (bPayment)
            {
                int r = Models.Payment.AfterPaymentUpdates(jsRet);

                return RedirectToAction("Confirmed", "Payment");
            }else
            {
                ViewBag.msg = -3;
                return View("Payment", payd);
            }


            return RedirectToAction("Index", "Packages");
        }
        public ActionResult Confirmed()
        {
            return View("Confirmed");
        }

        // not used
        [HttpPost]
        public ActionResult Promocode()
        {
            return PartialView("Payment");
        }

        public JsonResult PromocodeGet(string promocode)
        {
            Models.PromocodeResponse ret = new Models.PromoCode().PromocodeGet(promocode);
            Models.PromoCode.PromocodeAdd(ret);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }


    }
}