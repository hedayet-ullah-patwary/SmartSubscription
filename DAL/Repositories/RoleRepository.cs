using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public Role GetByName(string name)
        {
            return db.Roles.FirstOrDefault(x => x.Name == name);
        }


    }
}
