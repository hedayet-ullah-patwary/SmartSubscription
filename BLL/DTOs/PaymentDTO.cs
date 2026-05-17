using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
