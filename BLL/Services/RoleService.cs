using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class RoleService
    {
        private readonly DataAccessFactory data;

        public RoleService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<RoleDTO> GetAllRoles()
        {
            var mapper = MapperConfig.GetMapper();
            var roles = data.GetRoleRepository().GetAll();
            return mapper.Map<List<RoleDTO>>(roles);
        }

        public RoleDTO GetByName(string name)
        {
            var mapper = MapperConfig.GetMapper();
            var role = data.GetRoleRepository().GetByName(name);
            return mapper.Map<RoleDTO>(role);
        }

        public bool CreateRole(RoleDTO role)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Roles>(role);
            return data.GetRoleRepository().Create(entity);
        }

        public bool DeleteRole(int id)
        {
            return data.GetRoleRepository().Delete(id);
        }
    }
}