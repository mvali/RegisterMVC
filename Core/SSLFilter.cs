﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class SSLFilter : RequireHttpsAttribute
{
    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null)
        {
            throw new ArgumentNullException("filterContext");
        }

        //if (filterContext.HttpContext.Request.IsLocal)
        {
            // when connection to the application is local, don't do any HTTPS stuff
            return;
        }

        base.OnAuthorization(filterContext);
    }
}