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
            Employee empl = (Employee)_employeeRepository.Elements.First(e => e.EmployeeId == id);
           
            empl.Operations = empl.GetOperationsById(id);
            if(empl.Operations == null || empl.Operations.Count==0)
            {
                empl.Operations.Add(new Operation() { Name = "Операции отсутствуют" });
            };
     
            return PartialView(empl.Operations);
        }

        public ActionResult Register()
        {
            return View();
        }
    }
}