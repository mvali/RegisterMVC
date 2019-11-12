using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

public class JobControllerFilter : ActionFilterAttribute
{
    public string  DestControler { get; set; }
    public string DestAction { get; set; }
    //usage: [MyActionFilter(DestControler = "Value1", DestAction = "Value2")]

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!Register.Models.Job.Authorized(filterContext, null))
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary {{ "Controller", DestControler },
                                      { "Action", DestAction } });
        }

        base.OnActionExecuting(filterContext);
    }
}