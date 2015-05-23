using Microsoft.AspNet.SignalR;
using QueueStepchenko.Hubs;
using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Controllers
{
    public class TimerObjClass
    {
        // Used to hold parameters for calls to TimerTask.
        public int NumberCall;
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
        public string EmployeeName;
    }

    public class QueueController : Controller
    {
        IRepositoryQueue _queueRepository;
        IQueueHub _hub;

        public QueueController(IRepositoryQueue repo, IQueueHub hub)
        {
            _queueRepository = repo;
            _hub = hub;
        }

       
       public PartialViewResult Main()
       {
           if (HttpContext.User.Identity.IsAuthenticated)
           {
               //string[] roles = _userRepository.GetRolesForUser(HttpContext.User.Identity.Name);
               if (HttpContext.User.IsInRole("admin"))
               {
                   return PartialView("MainAdmin");
               }
               else
               {
                   if (HttpContext.User.IsInRole("employee"))
                   {
                       return PartialView("MainEmployee");
                   }
                   else
                   {
                       Queue queue = _queueRepository.GetQueue(HttpContext.User.Identity.Name);
                       if (queue == null || queue.Id == 0)
                       {
                           return PartialView("Label");
                       }
                       else
                       {
                           return PartialView("GetInQueue", queue);
                       }
                   }
               }
           }
           else
           {
               return PartialView("Label");
           }

       }


        [System.Web.Mvc.Authorize(Roles = "client")]
        public PartialViewResult GetInQueue(int id)
        {
              Queue queue = _queueRepository.GetIn(HttpContext.User.Identity.Name, id, StatesClient.Wait);

                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                string connectionId = _hub.GetConnectionIdByLogin(HttpContext.User.Identity.Name);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    context.Clients.Client(connectionId).disabledBtnInQueue();
                };
                context.Clients.All.changeCountClients(queue.Operation.CountClients, queue.Operation.Id);
                context.Clients.All.addClientInQueue(queue.PrevId, queue.Id, queue.Number, queue.Operation.Name, queue.Client.Name);

            
                return PartialView(queue);
            
        }

        public PartialViewResult GetOutQueue(int Id)
        {
            Operation operation =_queueRepository.GetOut(Id,StatesClient.GetOut);


            string connectionId = _hub.GetConnectionIdByLogin(HttpContext.User.Identity.Name);
            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
            if (!string.IsNullOrEmpty(connectionId))
            {
                context.Clients.Client(connectionId).enabledBtnInQueue();
            };
            context.Clients.All.changeCountClients(operation.CountClients, operation.Id);
            context.Clients.All.removeClientFromQueue(Id);

            return PartialView("Label");
        }


        public PartialViewResult List()
        {
            return PartialView(_queueRepository.GetListInQueue());
        }

  

        public void RunTimer()
        {
            TimerObjClass StateObj = new TimerObjClass();
            StateObj.TimerCanceled = false;
            StateObj.NumberCall = 0;
            StateObj.EmployeeName = HttpContext.User.Identity.Name;
            TimerCallback TimerDelegate = new TimerCallback(TimerTask);

            // Create a timer that calls a procedure every 2 seconds.
            // Note: There is no Start method; the timer starts running as soon as 
            // the instance is created.
            Timer TimerItem = new Timer(TimerDelegate, StateObj, 2000, 5000);

            // Save a reference for Dispose.
            StateObj.TimerReference = TimerItem;

            //// Run for ten loops.
            //while (StateObj.SomeValue < 10)
            //{
            //    // Wait one second.
            //    System.Threading.Thread.Sleep(1000);
            //}

            //// Request Dispose of the timer object.
            //StateObj.TimerCanceled = true;
        }

        private void TimerTask(object StateObj)
        {
            TimerObjClass State = (TimerObjClass)StateObj;
            // Use the interlocked class to increment the counter variable.
            Interlocked.Increment(ref State.NumberCall);

            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
           
            string connectionId = _hub.GetConnectionIdByLogin(State.EmployeeName);
            if (!string.IsNullOrEmpty(connectionId))
            {
                context.Clients.Client(connectionId).callClient();
            };

            if (State.NumberCall==3)
            // Dispose Requested.
            {
                State.TimerReference.Dispose();
               
            }
        }

    }
}