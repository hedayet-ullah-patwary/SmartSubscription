using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class PlanService
    {
        private readonly DataAccessFactory data;

        public PlanService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<PlanDTO> GetAllPlans(string sortBy = "name")
        {
            var mapper = MapperConfig.GetMapper();
            var plans  = data.GetPlanRepository().GetAll();

            plans = sortBy switch
            {
                "price"    => plans.OrderBy(p => p.Price).ToList(),
                "duration" => plans.OrderBy(p => p.DurationDays).ToList(),
                "active"   => plans.OrderByDescending(p => p.IsActive).ToList(),
                _          => plans.OrderBy(p => p.Name).ToList()
            };

            return mapper.Map<List<PlanDTO>>(plans);
        }

        public List<PlanDTO> GetPlansActiveorInactive(int isActive)
        {
            var mapper = MapperConfig.GetMapper();
            var plans  = data.GetPlanRepository().GetPlans(isActive);
            return mapper.Map<List<PlanDTO>>(plans);
        }

        public PlanDTO GetById(int id)
        {
            var mapper = MapperConfig.GetMapper();
            var plan   = data.GetRepository<Plan>().Find(id);
            return mapper.Map<PlanDTO>(plan);
        }

        public PlanDTO GetByName(string name)
        {
            var mapper = MapperConfig.GetMapper();
            var plan   = data.GetPlanRepository().GetByName(name);
            return mapper.Map<PlanDTO>(plan);
        }

        public bool CreatePlan(PlanDTO plan)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Plan>(plan);
            return data.GetRepository<Plan>().Create(entity);
        }

        public bool UpdatePlan(PlanDTO plan)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Plan>(plan);
            return data.GetRepository<Plan>().Update(entity);
        }

        public bool DeletePlan(int id)
        {
            return data.GetRepository<Plan>().Delete(id);
        }

        public bool HasSubscriptions(int planId)
        {
            return data.GetSubscriptionRepository().HasSubscriptionsForPlan(planId);
        }

        public bool ActivatePlan(int id)
        {
            return data.GetPlanRepository().ActivatePlan(id);
        }

        public bool DeactivatePlan(int id)
        {
            return data.GetPlanRepository().DeactivatePlan(id);
        }
    }
}
