using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueueStepchenko.Models;

namespace QueueStepchenko.Controllers
{
    public class HomeController : Controller
    {
       

        public ActionResult Index()
        {
           return View();
        }

        
    }
}