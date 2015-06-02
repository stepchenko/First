using Microsoft.AspNet.SignalR;
using QueueStepchenko.Hubs;
using QueueStepchenko.Models;
using QueueStepchenko.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace QueueStepchenko
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //this.AuthenticateRequest += (s, e) => { HttpContext.Current.Session["user"] = HttpContext.Current.User.Identity.Name; };
        }

        protected void Session_End(object sender, EventArgs e)
        {
            IRepositoryUser _userRepository = DependencyResolver.Current.GetService<IRepositoryUser>();

            int userId = _userRepository.LogOffUser(HttpContext.Current.User.Identity.Name);

            if (HttpContext.Current.User.IsInRole("employee"))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();

                context.Clients.All.logoffEmployee("#id_" + userId.ToString());
            };

         //   FormsAuthentication.SignOut();

        }
    }
}
