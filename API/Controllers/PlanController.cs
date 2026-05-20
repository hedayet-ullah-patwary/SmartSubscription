using BLL.DTOs;
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

        private IActionResult AdminOnly()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" ? null! : RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Index(string sortBy = "name")
        {
            var plans = service.GetAllPlans(sortBy);
            ViewBag.SortBy = sortBy;
            return View(plans);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;
            return View(new PlanDTO());
        }

        [HttpPost]
        public IActionResult Create(PlanDTO plan)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input";
                return View(plan);
            }

            var result = service.CreatePlan(plan);
            if (!result) 
            { 
                ViewBag.Error = "Plan creation failed"; 
                return View(plan); 
            }

            TempData["Success"] = "Plan created successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            var plan = service.GetById(id);
            if (plan == null) 
            { 
                TempData["Error"] = "Plan not found."; 
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        [HttpPost]
        public IActionResult Edit(PlanDTO plan)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid)
            { 
                ViewBag.Error = "Invalid input"; 
                return View(plan); 
            }

            var result = service.UpdatePlan(plan);
            TempData[result ? "Success" : "Error"] = result ? "Plan updated." : "Update failed.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
            {
                return guard;
            }

            if (service.HasSubscriptions(id))
            {
                TempData["Error"] = "Cannot delete plan with subscriptions. Deactivate it instead.";
                return RedirectToAction("Index");
            }

            var result = service.DeletePlan(id);
            TempData[result ? "Success" : "Error"] = result ? "Plan deleted." : "Delete failed.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Activate(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
            {
                return guard;
            }
            service.ActivatePlan(id);
            TempData["Success"] = "Plan activated";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Deactivate(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
            {
                return guard;
            }
            service.DeactivatePlan(id);
            TempData["Success"] = "Plan deactivated";
            return RedirectToAction("Index");
        }
    }
}
