﻿using Microsoft.AspNet.SignalR;
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

        public AccountController(IRepositoryUser userRepo, IRepositoryEmployee emplRepo) 
        {
            _userRepository = userRepo;
            _employeeRepository = emplRepo;
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

        
        
        public PartialViewResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewModel usermodel)
        {
            if (ModelState.IsValid)
            {
                User user= _userRepository.LogInUser(usermodel.Login, usermodel.Password); 
                if (user == null || user.Id==0)
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Login, true);

                    if (user.RoleName=="employee")
                    {

                        var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                        context.Clients.All.changeClass("#id_" + user.Id, "employeeLink");

                        string json = ListOperationsToJson(user.Id);

                        context.Clients.All.changeCountEmployees(json);

                    };
                    
                }
            }

            return RedirectToAction("Index", "Home"); 
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

        public ActionResult Register()
        {

            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User user = null;
        //        using (UserContext db = new UserContext())
        //        {
        //            user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
        //        }
        //        if (user == null)
        //        {
        //            using (UserContext db = new UserContext())
        //            {
        //                db.Users.Add(new User { Email = model.Name, Password = model.Password, RoleId = 2 });
        //                db.SaveChanges();

        //                user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
        //            }
        //            if (user != null)
        //            {
        //                FormsAuthentication.SetAuthCookie(model.Name, true);
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Пользователь с таким логином уже существует");
        //        }
        //    }
        //    return View(model);
        //}
        public ActionResult LogOff()
        {
            if(HttpContext.User.IsInRole("employee"))
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                int userId = _userRepository.LogOffUser(HttpContext.User.Identity.Name);
                context.Clients.All.changeClass("#id_" + userId, "employeeOffLink");

                string json = ListOperationsToJson(userId);

                context.Clients.All.changeCountEmployees(json);
            }
           

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}