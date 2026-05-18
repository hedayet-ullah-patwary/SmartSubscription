using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserService userService;
        private readonly PaymentService paymentService;
        private readonly PlanService planService;

        public AdminController(UserService userService, PaymentService paymentService, PlanService planService)
        {
            this.userService = userService;
            this.paymentService = paymentService;
            this.planService = planService;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalRevenue = paymentService.GetTotalRevenue();
            ViewBag.TotalPlans = planService.GetPlansActiveorInactive(1).Count;

            return View();
        }
    }
}