using System;
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
        private readonly UserRoleService userRoleService;

        public DashboardController(UserService userService, PlanService planService,
            PaymentService paymentService, SubscriptionService subscriptionService, UserRoleService userRoleService)
        {
            this.userService = userService;
            this.planService = planService;
            this.paymentService = paymentService;
            this.subscriptionService = subscriptionService;
            this.userRoleService = userRoleService;
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin") 
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalRevenue = paymentService.GetTotalRevenue();
            ViewBag.TotalPlans = planService.GetPlansActiveorInactive(1).Count;
            var users = userService.GetAllUsers();
            ViewBag.TotalUsers = users.Count;
            ViewBag.TotalSubscriptions = subscriptionService.GetAllSubscriptions().Count;

            var usersWithoutRoleList = users
                .Where(u => string.IsNullOrWhiteSpace(userRoleService.GetRoleNameByUserId(u.Id)))
                .ToList();
            ViewBag.UsersWithoutRole = usersWithoutRoleList.Count;
            ViewBag.UsersWithoutRoleList = usersWithoutRoleList;

            return View();
        }

        [HttpGet]
        public IActionResult UserDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User") 
                return RedirectToAction("Login", "Auth");

            var userId = HttpContext.Session.GetInt32("UserId");
            var user = userService.GetUserEntityById(userId.Value);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Login", "Auth");
            }

            if (user.IsEmailVerified == 0)
            {
                var existingCode = HttpContext.Session.GetString("EmailVerifyCode");
                var expiryValue = HttpContext.Session.GetString("EmailVerifyExpiry");
                var existingUserId = HttpContext.Session.GetInt32("EmailVerifyUserId");
                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var hasValidCode = !string.IsNullOrWhiteSpace(existingCode)
                    && long.TryParse(expiryValue, out var expiry)
                    && expiry > now
                    && existingUserId == user.Id;

                if (!hasValidCode)
                {
                    var newCode = Random.Shared.Next(100000, 999999).ToString();
                    var newExpiry = DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds();

                    HttpContext.Session.SetString("EmailVerifyCode", newCode);
                    HttpContext.Session.SetString("EmailVerifyExpiry", newExpiry.ToString());
                    HttpContext.Session.SetInt32("EmailVerifyUserId", user.Id);

                    existingCode = newCode;
                    expiryValue = newExpiry.ToString();
                }

                ViewBag.ShowEmailVerify = true;
                ViewBag.EmailVerifyCode = existingCode;
                ViewBag.EmailVerifyExpiry = expiryValue;
            }
            else
            {
                HttpContext.Session.Remove("EmailVerifyCode");
                HttpContext.Session.Remove("EmailVerifyExpiry");
                HttpContext.Session.Remove("EmailVerifyUserId");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult VerifyEmailCode(string code)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User") 
                return RedirectToAction("Login", "Auth");

            var userId = HttpContext.Session.GetInt32("UserId");
            var expected = HttpContext.Session.GetString("EmailVerifyCode");
            var expiryValue = HttpContext.Session.GetString("EmailVerifyExpiry");

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (userId == null || string.IsNullOrWhiteSpace(expected) ||
                !long.TryParse(expiryValue, out var expiry) || expiry <= now)
            {
                TempData["Error"] = "Verification code expired. Please resend and try again.";
                return RedirectToAction("UserDashboard");
            }

            if (!string.Equals(expected, code, StringComparison.Ordinal))
            {
                TempData["Error"] = "Invalid verification code. Please try again.";
                return RedirectToAction("UserDashboard");
            }

            var result = userService.VerifyEmail(userId.Value);
            if (!result)
            {
                TempData["Error"] = "Email verification failed. Please try again.";
                return RedirectToAction("UserDashboard");
            }

            HttpContext.Session.Remove("EmailVerifyCode");
            HttpContext.Session.Remove("EmailVerifyExpiry");
            HttpContext.Session.Remove("EmailVerifyUserId");

            TempData["Success"] = "Email verified successfully.";
            return RedirectToAction("UserDashboard");
        }

        [HttpPost]
        public IActionResult ResendEmailCode()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "User") 
                return RedirectToAction("Login", "Auth");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var newCode = Random.Shared.Next(100000, 999999).ToString();
            var newExpiry = DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds();

            HttpContext.Session.SetString("EmailVerifyCode", newCode);
            HttpContext.Session.SetString("EmailVerifyExpiry", newExpiry.ToString());
            HttpContext.Session.SetInt32("EmailVerifyUserId", userId.Value);

            TempData["Success"] = "A new verification code has been generated.";
            return RedirectToAction("UserDashboard");
        }

        [HttpGet]
        public IActionResult Analytics()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin") 
                return RedirectToAction("Login", "Auth");

            ViewBag.TotalRevenue = paymentService.GetTotalRevenue();
            ViewBag.TotalSubscriptions = subscriptionService.GetAllSubscriptions().Count;
            ViewBag.TotalPlans = planService.GetPlansActiveorInactive(1).Count;

            return View();
        }
    }
}
