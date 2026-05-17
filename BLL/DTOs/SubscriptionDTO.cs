using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.DTOs
{
    public class SubscriptionDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
