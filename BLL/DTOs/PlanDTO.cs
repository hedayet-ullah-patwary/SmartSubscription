using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class PlanDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int DurationDays { get; set; }

        public int ApiLimit { get; set; }

        public string Description { get; set; } = null!;

        public int IsActive { get; set; }
    }
}
