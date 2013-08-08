using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;


namespace LisaKatherine
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //            routes.MapRoute("Holding Page", "{*url}",
            //    new { controller = "Home", action = "Holding" }
            //);




            routes.MapRoute(
                "PhotoBlogDetails", // Route name
                "Photos/Details/{furl}/{id}", // URL with parameters
                new { controller = "Blog", action = "Details", id = UrlParameter.Optional, furl = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "WorkBlogDetails", // Route name
                "Work/Details/{furl}/{id}", // URL with parameters
                new { controller = "Blog", action = "Details", id = UrlParameter.Optional, furl = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "GeekBlogDetails", // Route name
                "Geek/Details/{furl}/{id}", // URL with parameters
                new { controller = "Blog", action = "Details", id = UrlParameter.Optional, furl = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                    "BlogDetails", // Route name
                    "{controller}/Details/{furl}/{id}/", // URL with parameters
                    new { controller = "Blog", action = "Details", id = UrlParameter.Optional, furl = UrlParameter.Optional } // Parameter defaults
                );

            //routes.MapRoute(
            //    "BlogList", // Route name
            //    "{controller}/BlogList/", // URL with parameters
            //    new { action = "Index" } // Parameter defaults
            //    );




            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //string cookie = FormsAuthentication.FormsCookieName;
            //HttpCookie httpcookie = Context.Request.Cookies[cookie];
            //if (httpcookie == null) return;
            //FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpcookie.Value);
            //if (ticket == null || ticket.Expired) return;

            //FormsIdentity identity = new FormsIdentity(ticket);
            //Users user = new UserService().GetUserFromTicket(ticket.UserData);

            //AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            //PrincipalPermission principalPerm = new PrincipalPermission(user.username, "Administrators", true);
            //principalPerm.Demand();

        }
    }
}