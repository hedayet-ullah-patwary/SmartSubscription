using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleService roleService;
        private readonly UserRoleService userRoleService;

        public RoleController(RoleService roleService, UserRoleService userRoleService)
        {
            this.roleService     = roleService;
            this.userRoleService = userRoleService;
        }

        private IActionResult AdminOnly()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" ? null! : RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Index(string sortBy = "name")
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;
            ViewBag.SortBy = sortBy;
            return View(roleService.GetAllRoles(sortBy));
        }

        [HttpPost]
        public IActionResult Create(RoleDTO role)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid) 
            { 
                TempData["Error"] = "Invalid input";
                return RedirectToAction("Index");
            }

            var result = roleService.CreateRole(role);
            TempData[result ? "Success" : "Error"] = result ? "Role created." : "Role creation failed (may already exist).";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            var role = roleService.GetById(id);
            if (role == null) 
            { 
                TempData["Error"] = "Role not found.";
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public IActionResult Edit(RoleDTO role)
        {
            var guard = AdminOnly();
            if (guard != null) 
                return guard;

            if (!ModelState.IsValid) 
            { 
                ViewBag.Error = "Invalid input"; 
                return View(role); 
            }

            var result = roleService.UpdateRole(role);
            TempData[result ? "Success" : "Error"] = result ? "Role updated." : "Update failed.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var guard = AdminOnly(); 
            if (guard != null) 
                return guard;

            var result = roleService.DeleteRole(id);
            TempData[result ? "Success" : "Error"] = result ? "Role deleted." : "Delete failed.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AssignRole(int userId, int roleId)
        {
            var guard = AdminOnly();
            if (guard != null) 
                return guard;

            var result = userRoleService.AssignRole(userId, roleId);
            TempData[result ? "Success" : "Error"] = result ? "Role assigned" : "Role assignment failed";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RemoveRole(int userId, int roleId)
        {
            var guard = AdminOnly();
            if (guard != null) 
                return guard;

            userRoleService.RemoveRole(userId, roleId);
            TempData["Success"] = "Role removed";
            return RedirectToAction("Index");
        }
    }
}
