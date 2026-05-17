using BLL.DTOs;
using DAL;
using DAL.EF.Tables;
using System.Collections.Generic;

namespace BLL.Services
{
    public class PaymentService
    {
        private readonly DataAccessFactory data;

        public PaymentService(DataAccessFactory data)
        {
            this.data = data;
        }

        public List<PaymentDTO> GetUserPayments(int userId)
        {
            var mapper = MapperConfig.GetMapper();
            var payments = data.GetPaymentRepository().GetUserPayments(userId);
            return mapper.Map<List<PaymentDTO>>(payments);
        }

        public decimal GetTotalRevenue()
        {
            return data.GetPaymentRepository().GetTotalRevenue();
        }

        public decimal GetRevenueByPlan(int planId)
        {
            return data.GetPaymentRepository().GetRevenueByPlan(planId);
        }

        public bool CreatePayment(PaymentDTO payment)
        {
            var mapper = MapperConfig.GetMapper();
            var entity = mapper.Map<Payment>(payment);
            return data.GetRepository<Payment>().Create(entity);
        }

        public List<PaymentDTO> GetFailedPayments()
        {
            var mapper = MapperConfig.GetMapper();
            var payments = data.GetPaymentRepository().GetFailedPayments();
            return mapper.Map<List<PaymentDTO>>(payments);
        }
    }
}