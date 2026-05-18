using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly SubscriptionService service;
        private readonly PlanService planService;
        private readonly PaymentService paymentService;

        public SubscriptionController(SubscriptionService service, PlanService planService, PaymentService paymentService)
        {
            this.service        = service;
            this.planService    = planService;
            this.paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var subs = service.GetSubscriptionsByUserId(userId.Value);

            var allPayments = paymentService.GetUserPayments(userId.Value);
            ViewBag.Payments = allPayments
                .GroupBy(p => p.SubscriptionId)
                .ToDictionary(g => g.Key, g => g.First());

            var allPlans = planService.GetAllPlans();
            ViewBag.Plans = allPlans.ToDictionary(p => p.Id, p => p);

            return View(subs);
        }

        [HttpGet]
        public IActionResult Checkout(int planId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var plan = planService.GetById(planId);
            if (plan == null) { TempData["Error"] = "Plan not found."; return RedirectToAction("Index", "Plan"); }

            if (service.IsSubscriptionActive(userId.Value))
            {
                TempData["Error"] = "You already have an active subscription. Cancel it first to switch plans.";
                return RedirectToAction("Index");
            }

            ViewBag.Plan = plan;
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(int planId, string paymentMethod)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var plan = planService.GetById(planId);
            if (plan == null) { TempData["Error"] = "Plan not found."; return RedirectToAction("Index", "Plan"); }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                ViewBag.Plan  = plan;
                ViewBag.Error = "Please select a payment method.";
                return View();
            }

            var sub = new SubscriptionDTO
            {
                UserId    = userId.Value,
                PlanId    = plan.Id,
                StartDate = DateTime.Now,
                EndDate   = DateTime.Now.AddDays(plan.DurationDays),
                Status    = "Active"
            };

            var payment = new PaymentDTO
            {
                UserId        = userId.Value,
                Amount        = plan.Price,
                PaymentMethod = paymentMethod,
                TransactionType = "Purchase",
                PaymentDate   = DateTime.Now
            };

            int newSubId = service.CreateSubscriptionWithPayment(sub, payment);

            if (newSubId < 0)
            {
                ViewBag.Plan  = plan;
                ViewBag.Error = "Subscription failed. Please try again.";
                return View();
            }

            TempData["Success"] = $"Successfully subscribed to {plan.Name}! Payment of ${plan.Price} recorded via {paymentMethod}.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(SubscriptionDTO sub)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            sub.UserId = userId.Value;
            var result = service.CreateSubscription(sub);

            TempData[result ? "Success" : "Error"] = result ? "Subscribed successfully." : "Subscription failed.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Cancel(int id)
        {
            service.CancelSubscription(id);
            TempData["Success"] = "Subscription cancelled.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Extend(int id, int days)
        {
            service.ExtendSubscription(id, days);
            TempData["Success"] = $"Subscription extended by {days} days.";
            return RedirectToAction("Index");
        }
    }
}
