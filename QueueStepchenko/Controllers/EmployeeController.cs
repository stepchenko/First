using QueueStepchenko.Hubs;
using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Controllers
{
    public class EmployeeController : Controller
    {
        IRepositoryEmployee _employeeRepository;

       public EmployeeController(IRepositoryEmployee repo)
        {
            _employeeRepository = repo;
        }

       
        public PartialViewResult ListEmployees()
        {
            return PartialView(_employeeRepository.GetList());
        }

        public PartialViewResult OperationsByEmployee(int id)
        {
            List<Operation> operations = _employeeRepository.GetOperationsById(id);

            if (operations == null || operations.Count == 0)
            {
                operations.Add(new Operation() { Name = "Операции отсутствуют" });
            };

            return PartialView(operations);
        }


        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "employee")]
        public ActionResult ChoiceOperations()
        {

            List<Operation> operations = _employeeRepository.GetOperationsForChoice(HttpContext.User.Identity.Name);

            return View(operations);
        }



        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "employee")]
        public ActionResult ChoiceOperations(string[] checkedValues, string action)
        {
            if (action == "OK")
            {
                _employeeRepository.SaveEmployeeOperations(HttpContext.User.Identity.Name, checkedValues);
            };

            return RedirectToAction("Index", "Home"); 
        }
    }
}