using BLL.DTOs;
using BLL.Services;
using DAL.EF.Tables;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionService service;

        public SubscriptionController(SubscriptionService service)
        {
            this.service = service;
        }

        public IActionResult Index(int userId)
        {
            var data = service.GetSubscriptionsByUserId(userId);
            return View(data);
        }

        [HttpPost]
        public IActionResult Create(SubscriptionDTO sub)
        {
            var result = service.CreateSubscription(sub);

            if (!result)
            {
                TempData["Error"] = "Subscription failed";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Subscription created";

            return RedirectToAction("Index", new { userId = sub.UserId });
        }

        public IActionResult Cancel(int id)
        {
            service.CancelSubscription(id);

            TempData["Success"] = "Subscription cancelled";

            return RedirectToAction("Index");
        }

        public IActionResult Extend(int id, int days)
        {
            service.ExtendSubscription(id, days);

            TempData["Success"] = "Subscription extended";

            return RedirectToAction("Index");
        }
    }
}