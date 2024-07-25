
using System.ComponentModel.DataAnnotations;


namespace DataModel.Models
{
    public class RegisterDto:LoginDto
    {
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Lastname { get; set; }
    }
}
