using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace API.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserService userService;
        private readonly RoleService roleService;
        private readonly UserRoleService userRoleService;

        public UserManagementController(UserService userService, RoleService roleService, UserRoleService userRoleService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.userRoleService = userRoleService;
        }

        // ─── Guard helper ─────────────────────────────────────────────
        private IActionResult AdminOnly()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin" ? null! : RedirectToAction("Login", "Auth");
        }

        // ─── INDEX  – list with sort + search ─────────────────────────
        [HttpGet]
        public IActionResult Index(string sortBy = "name", string search = "", int filterActive = -1)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            var users = userService.GetAllUsers();

            // search
            if (!string.IsNullOrWhiteSpace(search))
                users = users.Where(u =>
                    u.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            // filter active/inactive
            if (filterActive >= 0)
                users = users.Where(u => u.IsActive == filterActive).ToList();

            // sort
            users = sortBy switch
            {
                "email"     => users.OrderBy(u => u.Email).ToList(),
                "active"    => users.OrderByDescending(u => u.IsActive).ToList(),
                "created"   => users.OrderByDescending(u => u.CreatedAt).ToList(),
                _           => users.OrderBy(u => u.Name).ToList()
            };

            // attach roles
            var allRoles = roleService.GetAllRoles();
            ViewBag.AllRoles    = allRoles;
            ViewBag.UserRoles   = users.ToDictionary(
                u => u.Id,
                u => userRoleService.GetRoleNameByUserId(u.Id));

            ViewBag.SortBy       = sortBy;
            ViewBag.Search       = search;
            ViewBag.FilterActive = filterActive;

            return View(users);
        }

        // ─── CREATE GET ────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Create()
        {
            var guard = AdminOnly(); if (guard != null) return guard;
            ViewBag.Roles = new SelectList(roleService.GetAllRoles(), "Id", "Name");
            return View(new UserDTO());
        }

        // ─── CREATE POST ───────────────────────────────────────────────
        [HttpPost]
        public IActionResult Create(UserDTO model, int roleId)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input data.";
                ViewBag.Roles = new SelectList(roleService.GetAllRoles(), "Id", "Name");
                return View(model);
            }

            var result = userService.RegisterUser(model);
            if (result == null)
            {
                ViewBag.Error = "User creation failed. Email may already exist.";
                ViewBag.Roles = new SelectList(roleService.GetAllRoles(), "Id", "Name");
                return View(model);
            }

            // assign selected role
            if (roleId > 0)
            {
                var newUser = userService.GetAllUsers().FirstOrDefault(u => u.Email == model.Email);
                if (newUser != null)
                    userRoleService.AssignRole(newUser.Id, roleId);
            }

            TempData["Success"] = $"User '{model.Name}' created successfully.";
            return RedirectToAction("Index");
        }

        // ─── EDIT GET ──────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            var user = userService.GetUserById(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction("Index"); }

            var currentRoleName = userRoleService.GetRoleNameByUserId(id);
            var allRoles        = roleService.GetAllRoles();
            var currentRole     = allRoles.FirstOrDefault(r => r.Name == currentRoleName);

            ViewBag.Roles       = new SelectList(allRoles, "Id", "Name", currentRole?.Id);
            ViewBag.CurrentRole = currentRoleName;
            return View(user);
        }

        // ─── EDIT POST ─────────────────────────────────────────────────
        [HttpPost]
        public IActionResult Edit(UserDTO model, int roleId)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid input.";
                ViewBag.Roles = new SelectList(roleService.GetAllRoles(), "Id", "Name");
                return View(model);
            }

            var result = userService.UpdateUser(model);
            if (!result)
            {
                ViewBag.Error = "Update failed.";
                ViewBag.Roles = new SelectList(roleService.GetAllRoles(), "Id", "Name");
                return View(model);
            }

            // update role: remove old then assign new
            if (roleId > 0)
            {
                var existingRoles = userRoleService.GetRolesByUser(model.Id);
                foreach (var r in existingRoles)
                    userRoleService.RemoveRole(model.Id, r.Id);

                userRoleService.AssignRole(model.Id, roleId);
            }

            TempData["Success"] = "User updated successfully.";
            return RedirectToAction("Index");
        }

        // ─── DELETE ────────────────────────────────────────────────────
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            var result = userService.DeleteUser(id);
            TempData[result ? "Success" : "Error"] = result ? "User deleted." : "Delete failed.";
            return RedirectToAction("Index");
        }

        // ─── TOGGLE ACTIVE / DEACTIVATE ───────────────────────────────
        [HttpGet]
        public IActionResult ToggleActive(int id)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            var user = userService.GetUserById(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction("Index"); }

            user.IsActive = user.IsActive == 1 ? 0 : 1;
            userService.UpdateUser(user);

            TempData["Success"] = user.IsActive == 1 ? "User activated." : "User deactivated.";
            return RedirectToAction("Index");
        }

        // ─── ASSIGN ROLE (quick inline from list) ─────────────────────
        [HttpPost]
        public IActionResult AssignRole(int userId, int roleId)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            // remove existing
            var existing = userRoleService.GetRolesByUser(userId);
            foreach (var r in existing)
                userRoleService.RemoveRole(userId, r.Id);

            var result = userRoleService.AssignRole(userId, roleId);
            TempData[result ? "Success" : "Error"] = result ? "Role updated." : "Role assignment failed.";
            return RedirectToAction("Index");
        }

        // ─── DETAILS ──────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Details(int id)
        {
            var guard = AdminOnly(); if (guard != null) return guard;

            var user = userService.GetUserById(id);
            if (user == null) { TempData["Error"] = "User not found."; return RedirectToAction("Index"); }

            ViewBag.Role          = userRoleService.GetRoleNameByUserId(id);
            ViewBag.Subscriptions = userService.GetUserSubscriptions(id);
            ViewBag.Payments      = userService.GetUserPayments(id);
            return View(user);
        }
    }
}
