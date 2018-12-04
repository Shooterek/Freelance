using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Freelance
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Activate",
                url: "{controller}/{id}/activate",
                defaults: new { action = "Activate"}
            );

            routes.MapRoute(
                name: "AddOpinion",
                url: "Opinions/Add",
                defaults: new { controller = "Opinions", action = "Add" }
            );

            routes.MapRoute(
                name: "EditServiceType",
                url: "Admin/ServiceTypes/Edit/{id}",
                defaults: new {controller = "Admin", action = "EditServiceType"}
            );

            routes.MapRoute(
                name: "AddServiceType",
                url: "Admin/ServiceTypes/Add",
                defaults: new { controller = "Admin", action = "AddServiceType" }
            );

            routes.MapRoute(
                name: "Announcements",
                url: "{controller}/index/{page}",
                defaults: new {action = "Index"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
