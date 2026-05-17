using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IPaymentRepository
    {
        List<Payment> GetUserPayments(int userId);
        decimal GetTotalRevenue();
        decimal GetRevenueByPlan(int planId);
        List<Payment> GetFailedPayments();
    }
}
