using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService service;

        public UserController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult UserDashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = service.GetUserById(userId.Value);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = service.GetUserById(userId.Value);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("UserDashboard", "User");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = service.GetUserById(userId.Value);

            if (user == null)
            {
                TempData["Error"] = "User not found";
                return RedirectToAction("UserDashboard", "User");
            }
            return View(new UserDTO());
        }

        [HttpPost]
        public IActionResult EditProfile(UserDTO model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data";
                return View(model);
            }

            var result = service.UpdateUserProfile(model);

            if (!result)
            {
                ViewBag.Error = "Profile update failed";
                return View(model);
            }

            TempData["Success"] = "Profile updated successfully";

            return RedirectToAction("Profile", "User");
        }

        [HttpGet]
        public IActionResult SubscriptionHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var subscriptions = service.GetUserSubscriptions(userId.Value);

            return View(subscriptions);
        }

        [HttpGet]
        public IActionResult PaymentHistory()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var payments = service.GetUserPayments(userId.Value);

            return View(payments);
        }

        [HttpPost]
        public IActionResult DeleteAccount()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var result = service.DeleteUser(userId.Value);

            if (!result)
            {
                TempData["Error"] = "Account deletion failed";
                return RedirectToAction("Profile", "User");
            }

            HttpContext.Session.Clear();

            TempData["Success"] = "Account deleted successfully";

            return RedirectToAction("Register", "Auth");
        }
    }
}