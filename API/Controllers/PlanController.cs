/*using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PlanController : Controller
    {
        private readonly PlanService service;

        public PlanController(PlanService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            var plans = service.GetPlansActiveorInactive(1);
            return View(plans);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PlanDTO plan)
        {
            var result = service.CreatePlan(plan);

            if (!result)
            {
                ViewBag.Error = "Plan creation failed";
                return View(plan);
            }

            TempData["Success"] = "Plan created";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var plan = service.GetByName(id.ToString());

            if (plan == null)
                return NotFound();

            return View(plan);
        }

        [HttpPost]
        public IActionResult Edit(PlanDTO plan)
        {
            var result = service.UpdatePlan(plan);

            if (!result)
            {
                ViewBag.Error = "Update failed";
                return View(plan);
            }

            TempData["Success"] = "Plan updated";

            return RedirectToAction("Index");
        }

        public IActionResult Activate(int id)
        {
            service.ActivatePlan(id);
            return RedirectToAction("Index");
        }

        public IActionResult Deactivate(int id)
        {
            service.DeactivatePlan(id);
            return RedirectToAction("Index");
        }
    }
}*/