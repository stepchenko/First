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
            IRepositoryQueue _queueRepository = DependencyResolver.Current.GetService<IRepositoryQueue>();
            IQueueHub _hub = DependencyResolver.Current.GetService<IQueueHub>();

            Queue queue = _queueRepository.GetQueue(HttpContext.Current.User.Identity.Name);

            int userId = _userRepository.LogOffUser(HttpContext.Current.User.Identity.Name);

            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();

            bool isEmployee = HttpContext.Current.User.IsInRole("employee");

            if (isEmployee)
            {
                context.Clients.All.changeClass("#id_" + userId, "employeeOffLink");

            };

            if (queue != null && queue.Id > 0 && queue.StateClient != StatesClient.NoClient)
            {
                context.Clients.All.removeClientFromQueue(queue.Id);

                if (isEmployee)
                {
                    string connectionIdClient = _hub.GetConnectionIdByLogin(queue.Client.Login);
                    if (!string.IsNullOrEmpty(connectionIdClient))
                    {
                        context.Clients.Client(connectionIdClient).addMessageClient("Обслуживание завершено");
                    };

                }
                else
                {
                    context.Clients.All.changeCountClients(queue.Operation.CountClients, queue.Operation.Id);

                    if (queue.Employee != null && queue.Employee.EmployeeId > 0)
                    {
                        string connectionIdEmployee = _hub.GetConnectionIdByLogin(queue.Employee.Login);
                        if (!string.IsNullOrEmpty(connectionIdEmployee))
                        {
                            context.Clients.Client(connectionIdEmployee).addMessageEmployee("Клиент покинул очередь");
                        };
                    }
                }
            };

            FormsAuthentication.SignOut();

        }
    }
}
