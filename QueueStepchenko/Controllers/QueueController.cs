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
        public Queue Queue;
        public int NumberCall;
        public System.Threading.Timer TimerReference;
        public bool TimerCanceled;
        public IHubContext Context;
    }

    public class QueueController : Controller
    {
        IRepositoryQueue _queueRepository;
        IRepositoryOperation _operationRepository;
        IQueueHub _hub;

        public QueueController(IRepositoryQueue queueRepo, IRepositoryOperation operRepo, IQueueHub hub)
        {
            _queueRepository = queueRepo;
            _operationRepository = operRepo;
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
                   Queue queue = _queueRepository.GetQueue(HttpContext.User.Identity.Name);

                   if (HttpContext.User.IsInRole("employee"))
                   {
                       return PartialView("MainEmployee",queue);
                   }
                   else
                   { 
                       if (queue == null || queue.Id == 0)
                       {
                           return PartialView("Label");
                       }
                       else
                       {
                           return PartialView("MainClient", queue);
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

            if (queue.StateClient == StatesClient.Welcom)
            {
                context.Clients.All.addClientInQueue(queue.PrevId, queue.Id, queue.Number, queue.Operation.Name,
                                                    queue.Client.Name, (queue.StateClient == StatesClient.WaitExtra), "queueWelcom");
                CallClientTimer(queue);
            }
            else
            {
                string connectionId = _hub.GetConnectionIdByLogin(HttpContext.User.Identity.Name);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    context.Clients.Client(connectionId).disabledBtnInQueue();
                };
                context.Clients.All.changeCountClients(queue.Operation.CountClients, queue.Operation.Id);
                context.Clients.All.addClientInQueue(queue.PrevId, queue.Id, queue.Number, queue.Operation.Name,
                                                    queue.Client.Name, (queue.StateClient == StatesClient.WaitExtra), "queueWait");
            };
            
            return PartialView("MainClient",queue);
            
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


            Employee employee = _queueRepository.GetServicingEmployee(Id);
            if (employee!=null && employee.EmployeeId > 0)
            {
                connectionId = _hub.GetConnectionIdByLogin(employee.Login);
                if (!string.IsNullOrEmpty(connectionId))
                {
                    context.Clients.Client(connectionId).addMessageEmployee("Клиент покинул очередь");
                };
            };

            return PartialView("Label");
        }

      
        public PartialViewResult ListQueue()
        {
            return PartialView(_queueRepository.GetListInQueue());
        }

  

        public void CallClient()
        {
            if (_queueRepository.isCurrentUserInQueue(HttpContext.User.Identity.Name))
            {
                return;
            };

            
            Queue queue = _queueRepository.CallClient(HttpContext.User.Identity.Name);

            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
            
            string connectionIdEmployee = _hub.GetConnectionIdByLogin(queue.Employee.Login);

            if (queue.Client == null)
            {
                context.Clients.Client(connectionIdEmployee).noClient();
            }
            else
            {
                context.Clients.All.changeCountClients(queue.Operation.CountClients, queue.Operation.Id);
                context.Clients.All.changeClass("#queue_" + queue.Id + " > a", (queue.StateClient == StatesClient.Servicing) ? "queueServicing" : (queue.StateClient == StatesClient.Welcom) ? "queueWelcom" : "queueWait");

                CallClientTimer(queue);
            }
        }

        private void CallClientTimer(Queue queue)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();

            string connectionIdEmployee = _hub.GetConnectionIdByLogin(queue.Employee.Login);

            context.Clients.Client(connectionIdEmployee).callFromEmployee(queue.Client.Name, queue.Operation.Name);

            string connectionIdClient = _hub.GetConnectionIdByLogin(queue.Client.Login);

            if (!string.IsNullOrEmpty(connectionIdClient))
            {
                context.Clients.Client(connectionIdClient).callToClient(queue.Employee.Name);
            };

            TimerObjClass StateObj = new TimerObjClass()
            {
                TimerCanceled = false,
                NumberCall = 0,
                Queue = queue,
                Context = context
            };

            TimerCallback TimerDelegate = new TimerCallback(TimerTask);

            // Create a timer that calls a procedure every 2 seconds.
            // Note: There is no Start method; the timer starts running as soon as 
            // the instance is created.
            Timer TimerItem = new Timer(TimerDelegate, StateObj, 500, queue.TimeCall * 1000);

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

            StatesClient stateClient = _queueRepository.GetStateClient(State.Queue.Id);
            if (stateClient != StatesClient.Welcom)
            {
                State.TimerReference.Dispose();
            }
            else
            {
                if (State.NumberCall > State.Queue.MaxNumberCall)
                {
                    State.TimerReference.Dispose();

                    Operation operation = _queueRepository.GetOut(State.Queue.Id, StatesClient.GetOut);

                    string connectionIdEmployee = _hub.GetConnectionIdByLogin(State.Queue.Employee.Login);
                    if (!string.IsNullOrEmpty(connectionIdEmployee))
                    {
                        State.Context.Clients.Client(connectionIdEmployee).addMessageEmployee("Клиент не принял приглашение");
                    };
                    string connectionIdClient = _hub.GetConnectionIdByLogin(State.Queue.Client.Login);
                    if (!string.IsNullOrEmpty(connectionIdClient))
                    {
                        State.Context.Clients.Client(connectionIdClient).addMessageClient("Вы исключены из очереди");
                        State.Context.Clients.Client(connectionIdClient).changeClass("#BtnAccept", "noVisible");
                    };

                    State.Context.Clients.All.removeClientFromQueue(State.Queue.Id);

                }
                else
                {
                    string connectionIdEmployee = _hub.GetConnectionIdByLogin(State.Queue.Employee.Login);
                    if (!string.IsNullOrEmpty(connectionIdEmployee))
                    {
                        State.Context.Clients.Client(connectionIdEmployee).changeNumberCall(State.NumberCall, State.Queue.MaxNumberCall, State.Queue.TimeCall);
                    };
                    string connectionIdClient = _hub.GetConnectionIdByLogin(State.Queue.Client.Login);
                    if (!string.IsNullOrEmpty(connectionIdClient))
                    {
                        State.Context.Clients.Client(connectionIdClient).changeNumberCall(State.NumberCall, State.Queue.MaxNumberCall, State.Queue.TimeCall);
                    };
                }
            }
   
        }

        
        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "employee")]
        public PartialViewResult RedirectClient()
        {
            Queue queue = _queueRepository.GetQueue(HttpContext.User.Identity.Name);

            if (queue.StateClient == StatesClient.Servicing)
            {
                SelectList operations = new SelectList(_operationRepository.GetList(), "Id", "Name");
                RedirectClientViewModel viewModel = new RedirectClientViewModel(){Queue=queue, Operations=operations};

                return PartialView(viewModel);
            }
            else
            {
                return PartialView("MainEmployeeCenter",queue);
            }
        }

       
        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "employee")]
        public PartialViewResult RedirectClient(int queueId, int Id)
        {
            Queue queue = _queueRepository.RedirectClient(queueId, Id);

            if (queue != null && queue.Id > 0)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                context.Clients.All.removeClientFromQueue(queueId);

                context.Clients.All.changeCountClients(queue.Operation.CountClients, queue.Operation.Id);
                context.Clients.All.addClientInQueue(queue.PrevId, queue.Id, queue.Number, queue.Operation.Name,
                                                     queue.Client.Name, (queue.StateClient == StatesClient.WaitExtra),"queueWait");

                string connectionIdClient = _hub.GetConnectionIdByLogin(queue.Client.Login);
                if (!string.IsNullOrEmpty(connectionIdClient))
                {
                    context.Clients.Client(connectionIdClient).refreshMainClient();
                };
            };

            return PartialView("MainEmployeeCenter", queue);

        }

        public void DisableClient()
        {
            Queue queue = _queueRepository.GetQueue(HttpContext.User.Identity.Name);

            if (queue.StateClient == StatesClient.Servicing || queue.StateClient == StatesClient.Welcom)
            {
                Operation operation = _queueRepository.GetOut(queue.Id, StatesClient.Serviced);

                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                context.Clients.All.removeClientFromQueue(queue.Id);

                string connectionIdEmployee = _hub.GetConnectionIdByLogin(HttpContext.User.Identity.Name);
                context.Clients.Client(connectionIdEmployee).addMessageEmployee("Обслуживание клиента завершено");

                string connectionIdClient = _hub.GetConnectionIdByLogin(queue.Client.Login);
                if (!string.IsNullOrEmpty(connectionIdClient))
                {
                    context.Clients.Client(connectionIdClient).addMessageClient("Обслуживание завершено");
                };

            };
            
        }


        public void Accept(int id)
        {
            
            Queue queue = _queueRepository.Accept(id);

            if (queue.StateClient == StatesClient.Servicing)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                context.Clients.All.removeClientFromQueue(queue.Id);

                context.Clients.All.addClientInQueue(queue.PrevId, queue.Id, queue.Number, queue.Operation.Name,
                                                     queue.Client.Name, (queue.StateClient == StatesClient.WaitExtra), "queueServicing");


                string connectionIdClient = _hub.GetConnectionIdByLogin(queue.Client.Login);
                context.Clients.Client(connectionIdClient).servicingClient(queue.Employee.Name);

                string connectionIdEmployee = _hub.GetConnectionIdByLogin(queue.Employee.Login);
                if (!string.IsNullOrEmpty(connectionIdClient))
                {
                    context.Clients.Client(connectionIdEmployee).servicingEmployee(queue.Client.Name, queue.Operation.Name);
                };

            };

        }
    }
}