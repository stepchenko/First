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


        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult ViewEmployees()
        {
            List<EmployeeViewModel> employees = _employeeRepository.GetListForView();

            return View(employees);
        }


        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditEmployee(string login)
        {
            EmployeeViewModel employee = _employeeRepository.Get(login);

            return View(employee);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditEmployee(int EmployeeId, string Name, string Position)
        {
            _employeeRepository.Save(EmployeeId, Name, Position);

            return RedirectToAction("ViewEmployees");
        }


        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult RegisterEmployee()
        {
            EmployeeViewModel employee = new EmployeeViewModel();

            return View(employee);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult RegisterEmployee(EmployeeViewModel empl)
        {
            ModelState.Remove("OldPassword");

            if (ModelState.IsValid)
            {
                _employeeRepository.SaveWithPassword(empl.EmployeeId, empl.Login, empl.Name, empl.Position, empl.Password);
                return RedirectToAction("ViewEmployees");
            }
            else
            {
                return View(empl);
            }
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin,employee")]
        public ActionResult ProfileEmployee(string login)
        {
            EmployeeViewModel employee = _employeeRepository.Get(login);
            ViewBag.DivClass = "noVisible";

            return View(employee);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "admin,employee")]
        public ActionResult ProfileEmployee(EmployeeViewModel empl)
        {
            if (!empl.isChangePassword)
            {
                ModelState.Remove("OldPassword");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (ModelState.IsValid)
            {
                if (empl.isChangePassword)
                {
                    _employeeRepository.SaveWithPassword(empl.EmployeeId, empl.Login, empl.Name, empl.Position, empl.Password);
                }
                else
                {
                    _employeeRepository.Save(empl.EmployeeId, empl.Name, empl.Position);
                };

                return RedirectToAction("Index","Home");
            }
            else
            {
                if (empl.isChangePassword)
                {
                    ViewBag.DivClass="";
                }
                else
                {
                    ViewBag.DivClass = "noVisible";
                }
                return View(empl);
            }
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult DeleteEmployee(int id)
        {
            List<EmployeeViewModel> employees = _employeeRepository.Delete(id);

            return View("ViewEmployees", employees);

        }
    }
}