using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class RoleService
    {
        private readonly DataAccessFactory data;

        public RoleService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<RoleDTO> GetAllRoles(string sortBy = "name")
        {
            var mapper = MapperConfig.GetMapper();
            var roles  = data.GetRoleRepository().GetAll();

            roles = sortBy switch
            {
                "id"   => roles.OrderBy(r => r.Id).ToList(),
                _      => roles.OrderBy(r => r.Name).ToList()
            };

            return mapper.Map<List<RoleDTO>>(roles);
        }

        public RoleDTO GetById(int id)
        {
            var mapper = MapperConfig.GetMapper();
            var role   = data.GetRoleRepository().Find(id);
            return mapper.Map<RoleDTO>(role);
        }

        public RoleDTO GetByName(string name)
        {
            var mapper = MapperConfig.GetMapper();
            var role   = data.GetRoleRepository().GetByName(name);
            return mapper.Map<RoleDTO>(role);
        }


        public bool CreateRole(RoleDTO role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
                return false;

            // duplicate check
            var exists = data.GetRoleRepository().GetByName(role.Name);
            if (exists != null)
                return false;

            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Role>(role);
            return data.GetRoleRepository().Create(entity);
        }


        public bool UpdateRole(RoleDTO role)
        {
            if (role == null) 
                return false;

            var existing = data.GetRoleRepository().Find(role.Id);
            if (existing == null)
                return false;

            existing.Name = role.Name;
            return data.GetRoleRepository().Update(existing);
        }

        public bool DeleteRole(int id)
        {
            return data.GetRoleRepository().Delete(id);
        }

        public bool RoleExists(string name)
        {
            return data.GetRoleRepository().GetByName(name) != null;
        }
    }
}
