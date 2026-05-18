using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService service;

        public AuthController(UserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data";
                return View(dto);
            }

            var response = service.UserLogin(dto);

            if (response == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View(dto);
            }

            HttpContext.Session.SetInt32("UserId", response.Id);
            HttpContext.Session.SetString("UserName", response.Name);
            HttpContext.Session.SetString("UserEmail", response.Email);
            HttpContext.Session.SetString("UserRole", response.Role ?? "User");

            TempData["Success"] = "Login successful";

            if (response.Role == "Admin")
                return RedirectToAction("AdminDashboard", "Dashboard");

            if (response.Role == "User")
                return RedirectToAction("UserDashboard", "Dashboard");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserDTO model) 
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data";
                return View(model);
            }

            var result = service.RegisterUser(model);

            if (result == null)
            {
                ViewBag.Error = "Registration failed. Email may already be in use.";
                return View(model);
            }

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Logout successful";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}