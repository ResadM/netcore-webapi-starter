using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataModel.Entities
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public  string? Name { get; set; }
        [Required]
        public string? Lastname { get; set; }
    }
}
