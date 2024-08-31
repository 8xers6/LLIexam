using System.ComponentModel.DataAnnotations;

namespace LLI.Models
{
    public class RegisterViewModel
    {
        [Key]
        [Required]
        [StringLength(100, ErrorMessage = "Username cannot be longer than 100 characters.")]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string password { get; set; }

    }
}
