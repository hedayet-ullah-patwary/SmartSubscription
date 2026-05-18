using BLL.Services;
using DAL.EF.Tables;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserService userService;
        private readonly PlanService planService;
        private readonly PaymentService paymentService;
        private readonly SubscriptionService subscriptionService;

        public DashboardController(UserService userService, PlanService planService,
            PaymentService paymentService, SubscriptionService subscriptionService)
        {
            this.userService = userService;
            this.planService = planService;
            this.paymentService = paymentService;
            this.subscriptionService = subscriptionService;
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin") return RedirectToAction("Login", "Auth");

            ViewBag.TotalRevenue = paymentService.GetTotalRevenue();
            ViewBag.TotalPlans = planService.GetPlansActiveorInactive(1).Count;
            ViewBag.TotalUsers = userService.GetAllUsers().Count;
            ViewBag.TotalSubscriptions = subscriptionService.GetAllSubscriptions().Count;

            return View();
        }

        [HttpGet]
        public IActionResult UserDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User") return RedirectToAction("Login", "Auth");

            var userId = HttpContext.Session.GetInt32("UserId");
            var user = userService.GetUserEntityById(userId.Value);

            return View(user);
        }

        [HttpGet]
        public IActionResult Analytics()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin") return RedirectToAction("Login", "Auth");

            ViewBag.TotalRevenue = paymentService.GetTotalRevenue();
            ViewBag.TotalSubscriptions = subscriptionService.GetAllSubscriptions().Count;
            ViewBag.TotalPlans = planService.GetPlansActiveorInactive(1).Count;

            return View();
        }
    }
}
