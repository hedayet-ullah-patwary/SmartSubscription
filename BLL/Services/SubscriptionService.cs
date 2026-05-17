using BLL.DTOs;
using BLL.Mapper;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class SubscriptionService
    {
        private readonly DataAccessFactory data;

        public SubscriptionService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<SubscriptionDTO> GetSubscriptionsByUserId(int userId)
        {
            var mapper = MapperConfig.GetMapper();
            var subs = data.GetSubscriptionRepository().GetUserSubscriptions(userId);
            return mapper.Map<List<SubscriptionDTO>>(subs);
        }

        public SubscriptionDTO GetActiveSubscription(int userId)
        {
            var mapper = MapperConfig.GetMapper();
            var sub = data.GetSubscriptionRepository().GetActiveSubscription(userId);
            return mapper.Map<SubscriptionDTO>(sub);
        }

        public bool CreateSubscription(SubscriptionDTO sub)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Subcription>(sub);
            return data.GetRepository<Subcription>().Create(entity);
        }

        public bool CancelSubscription(int id)
        {
            return data.GetSubscriptionRepository().CancelSubscription(id);
        }

        public bool ExtendSubscription(int id, int days)
        {
            return data.GetSubscriptionRepository().ExtendSubscription(id, days);
        }

        public bool IsSubscriptionActive(int userId)
        {
            return data.GetSubscriptionRepository().IsSubscriptionActive(userId);
        }
    }
}