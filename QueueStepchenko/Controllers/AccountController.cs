using Microsoft.AspNet.SignalR;
using QueueStepchenko.Hubs;
using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QueueStepchenko.Controllers
{
    public class AccountController : Controller
    {
         IRepositoryUser _userRepository;

         public AccountController(IRepositoryUser repo)
        {
            _userRepository = repo;
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
                int userId = _userRepository.LogInUser(usermodel.Login, usermodel.Password); 
                if (userId>0)
                {
                    FormsAuthentication.SetAuthCookie(usermodel.Login, true);
                    
                    var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
                    context.Clients.All.loginEmployee("#id_"+userId.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return RedirectToAction("Index", "Home"); ;
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

            var context = GlobalHost.ConnectionManager.GetHubContext<QueueHub>();
            int userId = _userRepository.LogOffUser(HttpContext.User.Identity.Name);
            context.Clients.All.logoffEmployee("#id_" + userId.ToString());

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}