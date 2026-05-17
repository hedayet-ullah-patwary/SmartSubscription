using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class UserRoleRepository : Repository<Role>, IUserRoleRepository
    {
        public UserRoleRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public bool AssignRole(int userId, int roleId)
        {
            var userExists = db.Users.Any(x => x.Id == userId);
            if (!userExists)
                return false; // User does not exist

            var roleExists = db.Roles.Any(x => x.Id == roleId);
            if (!roleExists)
                return false; // Role does not exist

            var userRole = new UserRole()
            {
                UserId = userId,
                RoleId = roleId
            };

            db.UserRoles.Add(userRole);
            return db.SaveChanges() > 0;
        }

        public string GetRoleNameByUserId(int userId)
        {
            return  db.UserRoles.Include(x => x.Role)
                                .Where(x => x.UserId == userId)
                                .Select(x => x.Role.Name)
                                .FirstOrDefault();
        }

        public List<Role> GetRolesByUser(int userId)
        {
            return db.UserRoles.Where(x => x.UserId == userId)
                                .Select(x => x.Role).ToList();
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            return db.UserRoles.Any(x => x.UserId == userId &&
                                         x.Role.Name.ToLower() == roleName.ToLower());
        }

        public bool RemoveRole(int userId, int roleId)
        {
            var userRole = db.UserRoles.FirstOrDefault(x =>
           x.UserId == userId &&
           x.RoleId == roleId);

            if (userRole == null)
            {
               // Console.WriteLine("RemoveRole Failed: Role assignment not found");
                return false;
            }


            db.UserRoles.Remove(userRole);

            return db.SaveChanges() > 0;
        }
    }
}
