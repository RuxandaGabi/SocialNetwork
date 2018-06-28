using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNetwork
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "User", action = "Login"}
            );

            routes.MapRoute(
                name: "Register",
                url: "Register",
                defaults: new { controller = "User", action = "Registration" }
            );

            routes.MapRoute(
                name: "User",
                url: "User/{id}",
                defaults: new { controller = "Profile", action = "LastPosts", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Info",
                url: "User/{id}/Bio",
                defaults: new { controller = "Profile", action = "Bio", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Uploads",
                url: "User/{id}/Uploads",
                defaults: new { controller = "Profile", action = "Uploads", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Friends",
                url: "User/{id}/Friends",
                defaults: new { controller = "Profile", action = "Friends", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Pictures",
                url: "User/{id}/Gallery",
                defaults: new { controller = "Profile", action = "Gallery", id = UrlParameter.Optional },
                constraints: new { id = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
