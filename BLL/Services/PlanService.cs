using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PlanService
    {
        private readonly DataAccessFactory data;

        public PlanService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<PlanDTO> GetPlansActiveorInactive(int isActive)
        {
            var mapper = MapperConfig.GetMapper();
            var plans = data.GetPlanRepository().GetPlans(isActive);
            return mapper.Map<List<PlanDTO>>(plans);
        }

        public PlanDTO GetByName(string name)
        {
            var mapper = MapperConfig.GetMapper();
            var plan = data.GetPlanRepository().GetByName(name);
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