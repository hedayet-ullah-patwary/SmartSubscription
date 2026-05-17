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

        public IActionResult Index()
        {
            return View(service.GetAllFeatures());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FeatureDTO feature)
        {
            var result = service.CreateFeature(feature);

            if (!result)
            {
                ViewBag.Error = "Feature creation failed";
                return View(feature);
            }

            TempData["Success"] = "Feature created";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            service.DeleteFeature(id);

            TempData["Success"] = "Feature deleted";

            return RedirectToAction("Index");
        }
    }
}