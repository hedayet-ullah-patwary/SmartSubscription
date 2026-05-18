using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z0-9]).+$",
            ErrorMessage = "Password must include upper, lower, number, and special character.")]
        public string Password { get; set; }

        [Range(0, 1, ErrorMessage = "Email verification flag must be 0 or 1.")]
        public int IsEmailVerified { get; set; } = 0;

        [Range(0, 1, ErrorMessage = "Active flag must be 0 or 1.")]
        public int IsActive { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
