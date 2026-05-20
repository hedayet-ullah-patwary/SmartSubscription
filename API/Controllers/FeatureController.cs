using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FeatureController : Controller
    {
        private readonly FeatureService service;

        public FeatureController(FeatureService service)
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
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;
            ViewBag.SortBy = sortBy;
            return View(service.GetAllFeatures(sortBy));
        }

        [HttpPost]
        public IActionResult Create(FeatureDTO feature)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid) 
            { 
                ViewBag.Error = "Invalid input"; 
                return RedirectToAction("Index"); 
            }

            var result = service.CreateFeature(feature);
            TempData[result ? "Success" : "Error"] = result ? "Feature created" : "Feature creation failed";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guard = AdminOnly();
            if (guard != null) 
                return guard;

            var feature = service.GetById(id);
            if (feature == null) { 
                TempData["Error"] = "Feature not found."; 
                return RedirectToAction("Index"); 
            }
            return View(feature);
        }

        [HttpPost]
        public IActionResult Edit(FeatureDTO feature)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid) 
            { 
                ViewBag.Error = "Invalid input"; 
                return View(feature); 
            }

            var result = service.UpdateFeature(feature);
            TempData[result ? "Success" : "Error"] = result ? "Feature updated." : "Update failed.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            service.DeleteFeature(id);
            TempData["Success"] = "Feature deleted";
            return RedirectToAction("Index");
        }
    }
}
