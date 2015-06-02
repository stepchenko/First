using QueueStepchenko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueueStepchenko.Controllers
{
    public class SettingController : Controller
    {
        IRepositorySetting _settingRepository;

        public SettingController(IRepositorySetting repo)
        {
            _settingRepository = repo;
        }

        [HttpGet]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditSetting()
        {
            Setting setting = _settingRepository.Get();

            return View(setting);
        }

        [HttpPost]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult EditSetting(int NextNumberQueue, int NumberCall, int TimeCall)
        {
            _settingRepository.Save(NextNumberQueue, NumberCall, TimeCall);
                       
            return RedirectToAction("Index","Home");
        }

    }
}