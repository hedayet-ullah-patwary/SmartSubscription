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
        private readonly UserService userService;

        public SubscriptionController(SubscriptionService service, PlanService planService, PaymentService paymentService, UserService userService)
        {
            this.service        = service;
            this.planService    = planService;
            this.paymentService = paymentService;
            this.userService    = userService;
        }

        private IActionResult AdminOnly()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" ? null! : RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
                return RedirectToAction("Login", "Auth");

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
        public IActionResult AdminIndex()
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            var subs = service.GetAllSubscriptions();
            var plans = planService.GetAllPlans();
            var users = userService.GetAllUsers();

            ViewBag.Plans = plans.ToDictionary(p => p.Id, p => p);
            ViewBag.Users = users.ToDictionary(u => u.Id, u => u);

            return View(subs);
        }

        [HttpGet]
        public IActionResult AdminEdit(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null)
                return guard;

            var sub = service.GetById(id);
            if (sub == null)
            { 
                TempData["Error"] = "Subscription not found."; 
                return RedirectToAction("AdminIndex");
            }

            ViewBag.Plans = planService.GetAllPlans();
            return View(sub);
        }

        [HttpPost]
        public IActionResult AdminEdit(SubscriptionDTO sub, bool recalcEndDate)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input.";
                ViewBag.Plans = planService.GetAllPlans();
                return View(sub);
            }

            if (recalcEndDate)
            {
                var plan = planService.GetById(sub.PlanId);
                if (plan != null)
                {
                    sub.EndDate = sub.StartDate.AddDays(plan.DurationDays);
                }
            }

            var result = service.UpdateSubscription(sub);
            TempData[result ? "Success" : "Error"] = result ? "Subscription updated." : "Update failed.";
            return RedirectToAction("AdminIndex");
        }

        [HttpPost]
        public IActionResult AdminDelete(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (service.HasPaymentsForSubscription(id))
            {
                TempData["Error"] = "Cannot delete subscription with payments. Cancel it instead.";
                return RedirectToAction("AdminIndex");
            }

            var result = service.DeleteSubscription(id);
            TempData[result ? "Success" : "Error"] = result ? "Subscription deleted." : "Delete failed.";
            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public IActionResult Checkout(int planId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
                return RedirectToAction("Login", "Auth");

            var plan = planService.GetById(planId);
            if (plan == null) 
            { 
                TempData["Error"] = "Plan not found."; 
                return RedirectToAction("Index", "Plan"); 
            }

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
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var plan = planService.GetById(planId);
            if (plan == null) 
            { 
                TempData["Error"] = "Plan not found."; 
                return RedirectToAction("Index", "Plan");
            }

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
            if (userId == null) 
                return RedirectToAction("Login", "Auth");

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
