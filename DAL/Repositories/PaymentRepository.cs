using DAL.EF;
using DAL.EF.Tables;
using DAL.Interfaces;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(SmartSubcriptionsContext db) : base(db)
        {
        }

        public List<Payment> GetFailedPayments()
        {
            return db.Payments.Where(x => x.TransactionType == "Failed").ToList();
        }

        public decimal GetRevenueByPlan(int planId)
        {
            return db.Payments.Where(x => x.Subcription.PlanId == planId).Sum(x => x.Amount);
        }

        public decimal GetTotalRevenue()
        {
            return db.Payments.Sum(x => x.Amount);
        }

        public List<Payment> GetUserPayments(int userId)
        {
            return db.Payments.Where(x => x.UserId == userId).ToList();
        }
    }
}
