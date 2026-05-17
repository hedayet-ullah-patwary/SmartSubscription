using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface ISubscriptionRepository
    {
        Subcription GetActiveSubscription(int userId);
        List<Subcription> GetUserSubscriptions(int userId);
        bool IsSubscriptionActive(int userId);
        bool ExtendSubscription(int subscriptionId, int days);
        bool CancelSubscription(int subscriptionId);
    }
}
