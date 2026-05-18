using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System;
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

        public List<SubscriptionDTO> GetAllSubscriptions()
        {
            var mapper = MapperConfig.GetMapper();
            var subs = data.GetRepository<Subcription>().GetAll();
            return mapper.Map<List<SubscriptionDTO>>(subs);
        }

        public List<SubscriptionDTO> GetSubscriptionsByUserId(int userId)
        {
            var mapper = MapperConfig.GetMapper();
            var subs = data.GetSubscriptionRepository().GetUserSubscriptions(userId);
            return mapper.Map<List<SubscriptionDTO>>(subs);
        }

        public SubscriptionDTO GetById(int id)
        {
            var mapper = MapperConfig.GetMapper();
            var sub = data.GetRepository<Subcription>().Find(id);
            return mapper.Map<SubscriptionDTO>(sub);
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

        public bool UpdateSubscription(SubscriptionDTO sub)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Subcription>(sub);
            return data.GetRepository<Subcription>().Update(entity);
        }

        public bool DeleteSubscription(int id)
        {
            return data.GetRepository<Subcription>().Delete(id);
        }

        /// <summary>
        /// Creates a subscription AND its payment record in one call.
        /// Returns the new subscription Id on success, -1 on failure.
        /// </summary>
        public int CreateSubscriptionWithPayment(SubscriptionDTO sub, PaymentDTO payment)
        {
            var mapper = MapperConfig.GetMapper();

            // 1. Create subscription
            var subEntity = mapper.Map<Subcription>(sub);
            bool subCreated = data.GetRepository<Subcription>().Create(subEntity);
            if (!subCreated) return -1;

            // 2. Fetch the newly saved subscription to get its Id
            var saved = data.GetSubscriptionRepository().GetActiveSubscription(sub.UserId);
            if (saved == null) return -1;

            // 3. Build and save payment linked to this subscription
            var payEntity = new Payment
            {
                UserId         = sub.UserId,
                SubcriptionId  = saved.Id,
                Amount         = payment.Amount,
                PaymentMethod  = payment.PaymentMethod,
                TransactionType = "Purchase",
                PaymentDate    = DateTime.Now
            };

            bool payCreated = data.GetRepository<Payment>().Create(payEntity);
            if (!payCreated) return -1;

            return saved.Id;
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

        public bool HasPaymentsForSubscription(int subscriptionId)
        {
            return data.GetSubscriptionRepository().HasPaymentsForSubscription(subscriptionId);
        }
    }
}
