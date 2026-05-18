using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class FeatureDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Feature name is required.")]
        [StringLength(100, ErrorMessage = "Feature name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }
    }
}
