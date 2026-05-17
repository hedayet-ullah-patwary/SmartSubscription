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
            this.roleService = roleService;
            this.userRoleService = userRoleService;
        }

        public IActionResult Index()
        {
            return View(roleService.GetAllRoles());
        }

        [HttpPost]
        public IActionResult Create(RoleDTO role)
        {
            var result = roleService.CreateRole(role);

            if (!result)
            {
                TempData["Error"] = "Role creation failed";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Role created";

            return RedirectToAction("Index");
        }

        public IActionResult AssignRole(int userId, int roleId)
        {
            var result = userRoleService.AssignRole(userId, roleId);

            if (!result)
            {
                TempData["Error"] = "Role assignment failed";
            }
            else
            {
                TempData["Success"] = "Role assigned";
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveRole(int userId, int roleId)
        {
            userRoleService.RemoveRole(userId, roleId);

            TempData["Success"] = "Role removed";

            return RedirectToAction("Index");
        }
    }
}