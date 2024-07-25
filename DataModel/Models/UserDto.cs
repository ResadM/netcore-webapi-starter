
namespace DataModel.Models
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? UserRole { get; set; }
        public string? Token { get; set; }
    }
}
