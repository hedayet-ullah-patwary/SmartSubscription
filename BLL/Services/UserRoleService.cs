using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class UserRoleService
    {
        DataAccessFactory factory;

        public UserRoleService(DataAccessFactory factory)
        {
            this.factory = factory;
        }

        public bool AssignRole(int userId, int roleId)
        {
            return factory.GetUserRoleRepository().AssignRole(userId, roleId);
        }

        public bool RemoveRole(int userId, int roleId)
        {
            return factory.GetUserRoleRepository().RemoveRole(userId, roleId);
        }

        public List<RoleDTO> GetRolesByUser(int userId)
        {
            var data = factory.GetUserRoleRepository().GetRolesByUser(userId);

            var mapper = MapperConfig.GetMapper();

            return mapper.Map<List<RoleDTO>>(data);
        }

        public string GetRoleNameByUserId(int userId)
        {
            return factory.GetUserRoleRepository().GetRoleNameByUserId(userId);
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            return factory.GetUserRoleRepository().IsUserInRole(userId, roleName);
        }
    }
}