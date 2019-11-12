using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Register.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("ValidationError");
        }
        public ActionResult Validation()
        {
            return View("ValidationError");
        }
    }
}