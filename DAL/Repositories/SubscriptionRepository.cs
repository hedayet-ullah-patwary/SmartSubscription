using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class SubscriptionRepository : Repository<Subcription>, ISubscriptionRepository
    {
        public SubscriptionRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public bool CancelSubscription(int subscriptionId)
        {
            var subCheck = Find(subscriptionId) ;

            if (subCheck == null)
                return false;

            subCheck.Status = "Cancelled";

            return Update(subCheck);
        }

        public bool ExtendSubscription(int subscriptionId, int days)
        {
            var subCheck = Find(subscriptionId);
            if (subCheck == null) return false;

            subCheck.EndDate = subCheck.EndDate.AddDays(days);
            return Update(subCheck);
        }

        public Subcription GetActiveSubscription(int userId)
        {
            return db.Subcriptions.FirstOrDefault(x =>
                                                        x.UserId == userId &&
                                                        x.Status == "Active" &&
                                                        x.EndDate > DateTime.Now);
        }

        public List<Subcription> GetUserSubscriptions(int userId)
        {
            return db.Subcriptions.Where(x => x.UserId == userId).ToList();
        }

        public bool IsSubscriptionActive(int userId)
        {
            return db.Subcriptions.Any(x =>
                x.UserId == userId &&
                x.Status == "Active" &&
                x.EndDate > DateTime.Now);
        }

        public bool HasSubscriptionsForPlan(int planId)
        {
            return db.Subcriptions.Any(x => x.PlanId == planId);
        }

        public bool HasPaymentsForSubscription(int subscriptionId)
        {
            return db.Payments.Any(x => x.SubcriptionId == subscriptionId);
        }
    }
}
