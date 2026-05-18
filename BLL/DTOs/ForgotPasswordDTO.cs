using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}
