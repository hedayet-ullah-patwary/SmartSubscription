using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserRoleRepository
    {
        public string GetRoleNameByUserId(int userId);
        List<Role> GetRolesByUser(int userId);
        bool AssignRole(int userId, int roleId);
        bool RemoveRole(int userId, int roleId);
        bool IsUserInRole(int userId, string roleName);
    }
}
