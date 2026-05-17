using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        public PlanRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public bool ActivatePlan(int planId)
        {
            var plan = Find(planId);

            if (plan == null)
                return false;

            plan.IsActive = 1;
            return Update(plan);
        }

        public bool DeactivatePlan(int planId)
        {
            var plan = Find(planId);

            if (plan == null)
                return false;

            plan.IsActive = 0;
            return Update(plan);
        }

        public Plan GetByName(string name)
        {
            return db.Plans.FirstOrDefault(x => x.Name == name);
        }

        public List<Plan> GetPlans(int isActive)
        {
            return db.Plans.Where(x => x.IsActive == isActive).ToList();
        }
    }
}
