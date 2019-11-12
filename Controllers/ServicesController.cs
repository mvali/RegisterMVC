using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        [JobControllerFilter (DestControler ="Packages",DestAction ="Index")]
        public ActionResult Index()
        {
            int pkId = Models.Job.Session().PackageId;

            var resultsAll = new Register.Models.Service().GetByPackageCountry(pkId);
            return View("Services", resultsAll);
        }
    }
}