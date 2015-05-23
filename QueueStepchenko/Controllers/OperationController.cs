using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Controllers
{
    public class OperationController : Controller
    {
        IRepositoryOperation _operationRepository;

        public OperationController(IRepositoryOperation repo)
        {
            _operationRepository = repo;
        }

        
        public PartialViewResult List()
        {
            List<Operation> Operations = _operationRepository.GetList();
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole("client"))
            {
                ViewBag.isClient = true;
                ViewBag.isClientInQueue = _operationRepository.isCurrentClientInQueue(HttpContext.User.Identity.Name);
               
            }
            else
            {
                ViewBag.isClient=false; 
            }
            return PartialView(Operations);
        }
    }
}