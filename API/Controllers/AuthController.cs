using System;
using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserService service;
        private readonly RoleService roleService;
        private readonly UserRoleService userRoleService;
        private readonly IDistributedCache cache;

        public AuthController(UserService service, RoleService roleService, UserRoleService userRoleService, IDistributedCache cache)
        {
            this.service = service;
            this.roleService = roleService;
            this.userRoleService = userRoleService;
            this.cache = cache;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginDTO();

            if (Request.Cookies.TryGetValue("RememberEmail", out var email))
            {
                model.Email = email;
                model.RememberMe = true;
            }

            return View(model);
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
                var existing = service.GetUserByEmail(dto.Email);
                if (existing != null && existing.Password == dto.Password && existing.IsActive == 0)
                {
                    ViewBag.Error = "Your account is inactive. Please contact the administrator.";
                    return View(dto);
                }

                ViewBag.Error = "Invalid email or password";
                return View(dto);
            }

            if (string.IsNullOrWhiteSpace(response.Role))
            {
                ViewBag.Error = "Your registration is currently under review. Please wait for approval.";
                return View(dto);
            }

            HttpContext.Session.SetInt32("UserId", response.Id);
            HttpContext.Session.SetString("UserName", response.Name);
            HttpContext.Session.SetString("UserEmail", response.Email);
            HttpContext.Session.SetString("UserRole", response.Role);

            if (dto.RememberMe)
            {
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = Request.IsHttps
                };

                Response.Cookies.Append("RememberMe", response.Id.ToString(), options);
                Response.Cookies.Append("RememberEmail", dto.Email ?? string.Empty, options);
            }
            else
            {
                Response.Cookies.Delete("RememberEmail");
            }

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
            return View(new RegisterDTO());
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

            var role = roleService.GetByName("User");
            if (role == null)
            {
                roleService.CreateRole(new RoleDTO { Name = "User" });
                role = roleService.GetByName("User");
            }

            if (role != null)
            {
                userRoleService.AssignRole(result.Id, role.Id);
            }

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberMe");
            TempData["Success"] = "Logout successful";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordDTO());
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data";
                return View(dto);
            }

            var user = service.GetUserByEmail(dto.Email);
            if (user == null)
            {
                ViewBag.Error = "No account found with that email";
                return View(dto);
            }

            var demoCode = Random.Shared.Next(100000, 999999).ToString();

            HttpContext.Session.SetString("ResetCode", demoCode);
            HttpContext.Session.SetInt32("ResetUserId", user.Id);
            HttpContext.Session.SetString("ResetEmail", user.Email ?? string.Empty);

            ViewBag.DemoCode = demoCode;
            ViewBag.ShowCodeEntry = true;
            TempData["Success"] = "Demo code generated. Enter it to continue.";
            return View(dto);
        }

        [HttpPost]
        public IActionResult VerifyResetCode(string code)
        {
            var expected = HttpContext.Session.GetString("ResetCode");
            var userId = HttpContext.Session.GetInt32("ResetUserId");

            if (string.IsNullOrWhiteSpace(expected) || userId == null)
            {
                TempData["Error"] = "Demo code expired. Please request a new one.";
                return RedirectToAction("ForgotPassword");
            }

            if (!string.Equals(expected, code, StringComparison.Ordinal))
            {
                ViewBag.Error = "Invalid demo code";
                ViewBag.ShowCodeEntry = true;
                ViewBag.DemoCode = expected;
                return View("ForgotPassword", new ForgotPasswordDTO
                {
                    Email = HttpContext.Session.GetString("ResetEmail") ?? string.Empty
                });
            }

            var token = Guid.NewGuid().ToString("N");
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            };

            cache.SetString($"pwdreset:{token}", userId.Value.ToString(), options);
            HttpContext.Session.Remove("ResetCode");

            return RedirectToAction("ResetPassword", new { token });
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["Error"] = "Invalid or expired reset token.";
                return RedirectToAction("ForgotPassword");
            }

            var userId = cache.GetString($"pwdreset:{token}");
            if (string.IsNullOrWhiteSpace(userId))
            {
                TempData["Error"] = "Invalid or expired reset token.";
                return RedirectToAction("ForgotPassword");
            }

            return View(new ResetPasswordDTO { Token = token });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data";
                return View(dto);
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View(dto);
            }

            var userIdValue = cache.GetString($"pwdreset:{dto.Token}");
            if (string.IsNullOrWhiteSpace(userIdValue) || !int.TryParse(userIdValue, out var userId))
            {
                ViewBag.Error = "Invalid or expired reset token";
                return View(dto);
            }

            var result = service.UpdatePassword(userId, dto.NewPassword);
            if (!result)
            {
                ViewBag.Error = "Password reset failed";
                return View(dto);
            }

            cache.Remove($"pwdreset:{dto.Token}");
            TempData["Success"] = "Password updated. Please login.";
            return RedirectToAction("Login", "Auth");
        }
    }
}