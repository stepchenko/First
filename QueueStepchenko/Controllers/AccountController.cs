using Microsoft.AspNet.SignalR;
using QueueStepchenko.Hubs;
using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QueueStepchenko.Controllers
{
    public class AccountController : Controller
    {
        IRepositoryUser _userRepository;
        IRepositoryEmployee _employeeRepository;
        IRepositoryQueue _queueRepository;
        IQueueHub _hub;

        public AccountController(IRepositoryUser userRepo, IRepositoryEmployee emplRepo, IRepositoryQueue queueRepo, IQueueHub hub) 
        {
            _userRepository = userRepo;
            _employeeRepository = emplRepo;
            
            _queueRepository = queueRepo;
            _hub = hub;

        }

       
        public PartialViewResult LoginPartial()
         {
             if (HttpContext.User.Identity.IsAuthenticated)
             {
                User user = _userRepository.GetUserByLogin(HttpContext.User.Identity.Name);
                if (user == null)  
                {
                    return  PartialView("_LoginPartial");
                }
                else
                {
                    return PartialView("_LoggedPartial", user);
                } 
             }
             else
             {
                 return PartialView("_LoginPartial");
             }

         }


      [HttpGet]
        public PartialViewResult Login()
        {
            UserViewModel usermodel = new UserViewModel();

            return PartialView(usermodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
      public ActionResult Login(UserViewModel usermodel)
        {
            string errorMessage = string.Empty;

            if (ModelState.IsValid)
            {
                if (_userRepository.isVerifyPassword(usermodel.Login, usermodel.Password))
                {
                    User user = _userRepository.LogInUser(usermodel.Login); 

                    FormsAuthentication.SetAuthCookie(user.Login, true);

                    if (user.RoleName=="employee")
                    {

                        var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                        context.Clients.All.changeClass("#id_" + user.Id, "employeeLink");

                        string json = ListOperationsToJson(user.Id);

                        context.Clients.All.changeCountEmployees(json);

                    };

                }
                else
                {
                    errorMessage = "Неверный логин или пароль";
                }
            }
            else
            {
                errorMessage = "Неверный логин или пароль";
            }

            return RedirectToAction("Index", "Home", new { errorLogin = errorMessage }); 
        }


        private string ListOperationsToJson(int userId)
        {
            List<Operation> operations = _employeeRepository.GetOperationsById(userId); 

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (Operation operation in operations)
            {
                sb.Append("{\"id\":\"");
                sb.Append(operation.Id);
                sb.Append("\",\"count\":\"");
                sb.Append(operation.CountEmployees);
                sb.Append("\"},");
            };
            sb.Replace(',', ']', sb.Length - 1, 1);

            return sb.ToString();
        }


        [HttpGet]
        public ActionResult Register()
        {
            ClientViewModel client = new ClientViewModel();

            return View(client);
        }

        [HttpPost]
        public ActionResult Register(ClientViewModel client)
        {
            ModelState.Remove("OldPassword");

            if (ModelState.IsValid)
            {
                _userRepository.SaveWithPassword(client);

                FormsAuthentication.SetAuthCookie(client.Login, true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(client);
            }
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "client")]
        public ActionResult ProfileClient(string login)
        {
            ClientViewModel client = _userRepository.Get(login);

            client.Login = login;

            ViewBag.DivClass = "noVisible";

            return View(client);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "client")]
        public ActionResult ProfileClient(ClientViewModel client)
        {
            if (!client.isChangePassword)
            {
                ModelState.Remove("OldPassword");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (ModelState.IsValid)
            {
                if (client.isChangePassword)
                {
                     _userRepository.SaveWithPassword(client);
                }
                else
                {
                    _userRepository.Save(client.ClientId, client.Name, client.Email, client.Address,client.Phone);
                };

                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (client.isChangePassword)
                {
                    ViewBag.DivClass = "";
                }
                else
                {
                    ViewBag.DivClass = "noVisible";
                }
                return View(client);
            }
        }


        
        public ActionResult LogOff()
        {
            Queue queue = _queueRepository.GetQueue(HttpContext.User.Identity.Name);

            int userId = _userRepository.LogOffUser(HttpContext.User.Identity.Name);

            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();

            bool isEmployee = HttpContext.User.IsInRole("employee");

            if(isEmployee)
            {
                context.Clients.All.changeClass("#id_" + userId, "employeeOffLink");

                string json = ListOperationsToJson(userId);

                context.Clients.All.changeCountEmployees(json);
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

                    if(queue.Employee!=null && queue.Employee.EmployeeId>0)
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

            return RedirectToAction("Index", "Home");
        }
    }
}