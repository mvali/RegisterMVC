using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    [JobControllerFilter(DestControler = "SessionDateTime", DestAction = "Index")]
    public class OrderController : Controller
    {
        public ActionResult Index2()
        {
            return View("Order");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var jses = Models.Job.Session();
            var jInfo = jses.JobPersonalInfo;
            if (LoggedProfile.logged.Id > 0)
            {
                jInfo.FirstName = LoggedProfile.logged.FirstName;
                jInfo.LastName = LoggedProfile.logged.LastName;
                jInfo.JobEmail = LoggedProfile.logged.Email;
                jInfo.Password = LoggedProfile.logged.Password;
                jInfo.Password2 = LoggedProfile.logged.Password;
                jInfo.Phone = LoggedProfile.logged.Phone;
            }
            return View("Order", jInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Order(Models.JobInfo ji)
        {
            var j = Models.Job.Session();
            ViewBag.msg = 0;
            if (j.Paid)
            {
                ViewBag.msg = -10;
                return View("Order", ji);
            }
            if (!ji.TermsConditionsAgree)
            {
                ViewBag.msg = -11;
                return View("Order", ji);
            }


            if (ModelState.IsValid)
            {
                j.JobPersonalInfo = ji;
                Models.Job.Update(j);
                int job_id_parent = LoggedProfile.logged.Id;
                string servicesToPay = "";
                foreach (var x in j.ServicesId)
                {
                    servicesToPay += (servicesToPay.Length > 0 ? "," : "") + x.ToString();
                }

                int r = new Jobs().Job_AddCheck(j.Id, job_id_parent, ji.JobEmail, j.Zip.ZipCode, j.Zip.StateCode, servicesToPay, j.Zip.CountryId);
                if (r < 0)
                {
                    ViewBag.msg = r;
                    return View("Order", ji);
                }
                else// jobCheck OK
                {
                    bool abs = j.Zip.AddressBillingSame;
                    string referrer = Models.Job.RefererGet();// to check from cookie
                    int fk_c_id = j.CurrencyId;// to see
                    decimal c_value = j.CurrencyValue;

                    string visitDateCk = Models.Job.VisitorSignupCookieGet("visitdate");
                    string visitDate = !String.IsNullOrWhiteSpace(visitDateCk) ? DateTime.Parse(Server.UrlDecode(visitDateCk)).ToString() : "";

                    int bannerId = Models.Job.BannerGet();
                    int jobMailingList = 0;

                    var sl = new List<Models.Service>();
                    var p = new Models.Package();
                    decimal promoCodeAmount = 0;
                    decimal price = Models.Job.AmountTotal(false, ref sl, ref p, ref promoCodeAmount);
                    DateTime sessTimeDate = j.SessionDateTime.AddHours(j.SessionTime);

                    int r2 = new Jobs().Job_Add
                        (j.Id, ji.FirstName, ji.LastName, ji.JobEmail, ji.Password, abs ? j.Zip.AddressText : j.Zip.AddressTextBill, abs ? j.Zip.CountryId : j.Zip.CountryIdBill, abs ? j.Zip.City : j.Zip.CityBill,
                        abs ? j.Zip.StateCode : j.Zip.StateCodeBill, abs ? j.Zip.ZipCode : j.Zip.ZipCodeBill, ji.Phone, j.Promocode !=null ? j.Promocode.name : null, sessTimeDate.ToString(), j.PhotographerId, referrer, fk_c_id, c_value, price, j.PackageId,
                        visitDate, bannerId, jobMailingList, promoCodeAmount, job_id_parent, abs ? "" : j.Zip.AddressText, abs ? "" : j.Zip.City, abs ? "" : j.Zip.ZipCode, abs ? "" : j.Zip.StateCode, abs ? -1 : j.Zip.CountryId
                        );

                    if (r2 < 0)
                    {
                        ViewBag.msg = r2;
                        return View("Order", ji);
                    }
                    else// everything ok
                    {
                        j.Id = r2;
                        Models.Job.Update(j);

                        return RedirectToAction("Index", "Payment");
                    }
                }
            }
            else
            {
                return View("Order", ji);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            var x = new Models.JobLogin();
            x.JobEmail = "";
            x.Password = "";
            return PartialView("Login", x);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Models.JobLogin jl)
        {
            if (ModelState.IsValid)
            {
                LoggedProfile lp = LoggedProfile.LoggedDbRead(jl.JobEmail, jl.Password);
                if (lp.Id > 0)
                {
                    ViewBag.msg = 1;
                    return PartialView("Login", jl);
                }
                else
                {
                    ViewBag.msg = -2;
                    return PartialView("Login", jl);
                }
            }
            else
            {
                ViewBag.msg = -1;
                return PartialView("Login", jl);
            }
        }
        public ActionResult Logout()
        {
            LoggedProfile.LoggedDestroy();
            return RedirectToAction("Index", "Order");
        }
        
    }
}