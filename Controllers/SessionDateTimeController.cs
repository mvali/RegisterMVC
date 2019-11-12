using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    [JobControllerFilter (DestControler="Services", DestAction ="Index")]
    public class SessionDateTimeController : Controller
    {
        // GET: SessionDateTime
        public ActionResult Index()
        {
            return View("SessionDate");
        }

        [HttpGet]
        public ActionResult SessionDate()
        {
            DateTime sd = Models.SessionDate.Session();

            ViewBag.chosenDate = String.Format("{0},{1},{2}", sd.Year, sd.Month - 1, sd.Day);
            ViewBag.chosenDateN = String.Format("yyyy/MM/dd", sd);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SessionDate(Models.SessionDate sd)
        {
            DateTime sd2 = DateTime.Parse(sd.PhotoSessionDateS).Date;
            int days = (sd2 - DateTime.Now.Date).Days;

            if (ModelState.IsValid || days >=3)
            {
                int r = Models.Job.SessionDateTimeUpdate(sd.PhotoSessionDateS);

                return RedirectToAction("SessionTime", "SessionDateTime");
            }
            else
            {
                ViewBag.chosenDate = String.Format("{0},{1},{2}", sd2.Year, sd2.Month - 1, sd2.Day);
                ViewBag.chosenDateN = String.Format("yyyy/MM/dd", sd2);
                return View("SessionDate", sd);
            }
        }

        public ActionResult SessionTime()
        {
            ViewBag.chosenTime = Models.SessionDate.SessionTime();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SessionTime(Models.SessionDate sd)
        {
            int r = Models.Job.SessionTimeUpdate(sd.PhotoSessionTime);

            return RedirectToAction("Index", "Order");
        }
    }
}