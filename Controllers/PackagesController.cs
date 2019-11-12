using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class PackagesController : Controller
    {
        // GET: Packages
        [JobControllerFilter(DestControler ="Addresses",DestAction ="Index")]
        public ActionResult Index()
        {
            var resultsAll = new Register.Models.Package().GetByCountry();

            return View("Packages", resultsAll);
        }
        public ActionResult Packages()
        {
            var resultsAll = new Register.Models.Package().GetByCountry();

            return View("Packages", resultsAll);
        }
    }
}