using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Register
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*allico}", new { allico = @".*\.ico(/.*)?" });

            routes.MapRoute(name: "About", url: "about", defaults: new { Controller = "Home", Action = "about" });
            routes.MapRoute(name: "Blog2", url: "blog-single", defaults: new { Controller = "Pages", Action = "blog2" });

            routes.MapRoute(name: "Services", url: "services/{pkid}", defaults: new { Controller = "Services", Action = "Index", pkid = "" });
            routes.MapRoute(name: "Address3", url: "Address/ph/{ph}", defaults: new { Controller = "Addresses", Action = "Index" });
            routes.MapRoute(name: "Address4", url: "Address/zip/{zip}", defaults: new { Controller = "Addresses", Action = "Index" });
            routes.MapRoute(name: "Address", url: "Address/zip/{zip}/ph/{ph}", defaults: new { Controller = "Addresses", Action = "Index" });
            routes.MapRoute(name: "Address2", url: "Address/{zip}", defaults: new { Controller = "Addresses", Action = "Index", zip = "" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{zip}",
                defaults: new { controller = "Addresses", action = "Index", zip = UrlParameter.Optional }
            );
        }
    }
}
