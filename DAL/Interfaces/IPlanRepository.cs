using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Plan GetByName(string name);
        bool ActivatePlan(int planId);
        bool DeactivatePlan(int planId);
        List<Plan> GetPlans(int isActive); // Get active and inactive plans
    }
}
