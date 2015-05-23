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
            _employeeRepository.Elements = _employeeRepository.GetList();
        }

       
        public PartialViewResult List()
        {
            
            return PartialView(_employeeRepository.Elements);
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

        public ActionResult Register()
        {
            return View();
        }
    }
}