using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class SubscriptionDTO
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "User is required.")]
        public int UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Plan is required.")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; }
    }
}
