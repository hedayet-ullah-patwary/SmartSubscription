using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string Name { get; set; }
    }
}
