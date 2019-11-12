using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class ValidationException : FilterAttribute, IExceptionFilter
{

    public void OnException(ExceptionContext filterContext)
    {
        if (!filterContext.ExceptionHandled && filterContext.Exception is HttpRequestValidationException)
        {
            filterContext.Result = new RedirectResult("~/Error/Validation");
            filterContext.ExceptionHandled = true;
        }
    }
}