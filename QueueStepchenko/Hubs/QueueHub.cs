using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using QueueStepchenko.Models;
using System.Web.Security;

namespace QueueStepchenko.Hubs
{
    public class QueueHub : Hub
    {
       static List<UserHub> Users = new List<UserHub>();

       public void CallClient()
       {
            Clients.All.callClient("admin1");
           UserHub user = Users.FirstOrDefault(x => x.Login == "petr");
           if (user != null)
           {
               Clients.Client(user.ConnectionId).CallClient("admin2");
              
           }
       }


        public void Connect()
        {
            if(Context.User.Identity.IsAuthenticated)
            {
                UserHub user = new UserHub();
                user.ConnectionId = Context.ConnectionId;
                user.Login = Context.User.Identity.Name;
                Users.Add(user);
            }
        }

        public string GetConnectionIdByLogin(string login)
        {
            UserHub user = Users.FirstOrDefault(u => u.Login == login);
            if (user==null)
            {
                return "";
            }
            else
            {
                return user.ConnectionId;
            }
            
        }

        public bool isLoginUser(string login)
        {
            return Users.Any(u => u.Login == login);
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            UserHub user = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                Users.Remove(user);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}