using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class AddressesController : Controller
    {
        // GET: Addresses
        [HttpGet]
        public ActionResult Index(string zip, string ph)
        {
            var zipv = zip;
            var jses = Models.Job.Session();
            var mzip= jses.Zip;
            var mph = jses.PhotographerId;
            var zipAction = "";
            var zipbill = "";

            if (mzip != null)
            {
                if (!String.IsNullOrWhiteSpace(mzip.ZipCode))
                {
                    zipv = mzip.ZipCode;
                    zipAction = "sv";
                }
                if (!String.IsNullOrWhiteSpace(mzip.ZipCodeBill))
                {
                    zipbill = mzip.ZipCodeBill;
                }
            }
            if (String.IsNullOrWhiteSpace(zipAction) && !String.IsNullOrWhiteSpace(zip))
            {
                zipAction = "url";
            }
            if (!String.IsNullOrWhiteSpace(ph))
            {
                Models.Photographer.Update(Utils.Str2Int(ph));
            }
            else
            {
                //Models.Photographer.Update(5945);
            }

            ViewBag.bzip = mzip;
            ViewBag.zip = zipv;
            ViewBag.zipBill = zipbill;
            ViewBag.zipAction = zipAction;

            return View("Address", mzip);
        }

        public JsonResult ZipDetails(string zipcode)
        {
            var ret = new Models.ZipDetails().GetAll(zipcode);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.ZipDetails zp)
        {
            var ju = Models.Job.Session();
            if (ju.Paid)
                return View("Address", zp);

            if (ModelState.IsValid) 
            {
                ViewBag.zip = zp.ZipCode;

                ju.Zip = zp;
                ju.CountryId = zp.AddressBillingSame ? zp.CountryId : zp.CountryIdBill;

                System.Data.DataTable dt = new System.Data.DataTable();
                int r = new Packages().Packages_get_byCountryId(ju.CountryId, ref dt);
                if (dt.Rows.Count > 0)
                {
                    decimal currencyValue = Utils.Str2Decimal(dt.Rows[0]["c_value"].ToString());
                    ju.CurrencyValue = currencyValue;
                    ju.Currency = dt.Rows[0]["c_name"].ToString();
                    ju.CurrencySymbol = dt.Rows[0]["c_symbol"].ToString();
                    ju.CurrencyId = Utils.Str2Int(dt.Rows[0]["c_id"].ToString());
                }

                // just for testing
                //int re = new EmailServer().EmailSend("OptionalService", new MailType("aaa@aaa.com"), new List<EmailParameter> { }, null);

                Models.Job.Update(ju);

                return RedirectToAction("Index", "Packages");
            }
            else
            {
                return View("Address", zp);
            }
        }
    }
}