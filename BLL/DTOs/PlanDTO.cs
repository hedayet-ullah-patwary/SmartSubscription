using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class PlanDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plan name is required.")]
        [StringLength(100, ErrorMessage = "Plan name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(1, 3650, ErrorMessage = "Duration must be between 1 and 3650 days.")]
        public int DurationDays { get; set; }

        [Range(0, 1000000, ErrorMessage = "API limit must be between 0 and 1,000,000.")]
        public int ApiLimit { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; } = null!;

        [Range(0, 1, ErrorMessage = "Active flag must be 0 or 1.")]
        public int IsActive { get; set; }
    }
}
