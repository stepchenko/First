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
        IRepositoryQueue _queueRepository;

        public OperationController(IRepositoryOperation operRepo, IRepositoryQueue queueRepo)
        {
            _operationRepository = operRepo;
            _queueRepository = queueRepo;
        }

        
        public PartialViewResult ListOperations()
        {
            List<Operation> Operations = _operationRepository.GetList();
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.IsInRole("client"))
            {
                ViewBag.isClient = true;
                ViewBag.isClientInQueue = _queueRepository.isCurrentUserInQueue(HttpContext.User.Identity.Name);
               
            }
            else
            {
                ViewBag.isClient=false; 
            }
            return PartialView(Operations);
        }

     
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult ViewOperations()
        {
            List<OperationViewModel> operations = _operationRepository.GetListForView();

            return View(operations);
        }


        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditOperation(int? id)
        {
            OperationViewModel operation;

            if (id == null || id==0)
            {
                operation = new OperationViewModel();
            }
            else
            {
                 operation = _operationRepository.Get(id.GetValueOrDefault());
            };
            

            return View(operation);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditOperation(int id, string name, string action)
        {
            if (action == "OK")
            {
                _operationRepository.Save(id, name);
            }

            return RedirectToAction("ViewOperations");
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult DeleteOperation(int id)
        {
            List<OperationViewModel> operations = _operationRepository.Delete(id);

            return View("ViewOperations",operations);

        }
    }
}