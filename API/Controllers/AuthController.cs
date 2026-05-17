using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService service;

        public AuthController(UserService service)
        {
            this.service = service;
        }

        // HOME PAGE
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // LOGIN PAGE
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
              // MODEL VALIDATION
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Invalid input data";
                    return View(dto);
                }

                var response = service.UserLogin(dto);


                // LOGIN FAILED
                if (response == null)
                {
                    ViewBag.Error = "Invalid email or password";
                    return View(dto);
                }

                // STORE SESSION
                HttpContext.Session.SetInt32("UserId", response.Id);
                HttpContext.Session.SetString("UserName", response.Name);
                HttpContext.Session.SetString("UserEmail", response.Email);
                HttpContext.Session.SetString("UserRole", response.Role);

                // SUCCESS MESSAGE
                TempData["Success"] = "Login successful";

                // ROLE BASED REDIRECTION
                if (response.Role == "Admin")
                    return RedirectToAction("AdminDashboard", "Dashboard");


                if (response.Role == "User")
                    return RedirectToAction("UserDashboard", "Dashboard");

                // DEFAULT REDIRECT
                return RedirectToAction( "Index","Home");
  
        }

        // REGISTER PAGE
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER USER
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO model)
        {
                // MODEL VALIDATION
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Invalid input data";
                    return View(model);
                }

                // REGISTER USER
                var result = service.RegisterUser(model);

                // REGISTRATION FAILED
                if (result == null)
                {
                    ViewBag.Error = "Registration failed";
                    return View(model);
                }

                // SUCCESS MESSAGE

                TempData["Success"] = "Registration successful. Please login.";

                // REDIRECT TO LOGIN

                return RedirectToAction( "Login", "Auth");
            
        }

        // LOGOUT
        [HttpGet]
        public IActionResult Logout()
        {

                HttpContext.Session.Clear();

                TempData["Success"] = "Logout successful";

                return RedirectToAction("Login", "Auth");
         
        }

        // ACCESS DENIED
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // FORGOT PASSWORD PAGE
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // RESET PASSWORD PAGE
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}