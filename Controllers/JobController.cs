using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            return View("Submited");
        }
        public ActionResult Submited()
        {
            return View();
        }

        public ActionResult NavPrice()
        {
            Register.Models.Job j = Register.Models.Job.Session();
            ViewBag.JobAmount = Models.Job.AmountTotal().ToString("C");//0.00 C
            ViewBag.Currency = j.Currency !=null ? j.Currency : "";
            return PartialView();
        }
        public JsonResult ServiceUpdate(int serviceid, int opr)
        {
            string amount = (Models.Job.j.Paid ? Models.Job.AmountTotal() : Models.Job.ServiceUpdate(serviceid, opr)).ToString("C");
            return Json(amount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PackageUpdate(int pkid)
        {
            string amount = (Models.Job.j.Paid ? Models.Job.AmountTotal() : Models.Package.PackageUpdate(pkid)).ToString("C");
            return Json(amount, JsonRequestBehavior.AllowGet);
        }
    }
}