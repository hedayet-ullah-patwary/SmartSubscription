using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class PaymentDTO
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "User is required.")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Subscription is required.")]
        public int SubscriptionId { get; set; }

        [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        [StringLength(50, ErrorMessage = "Payment method cannot exceed 50 characters.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        [StringLength(50, ErrorMessage = "Transaction type cannot exceed 50 characters.")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; }
    }
}
