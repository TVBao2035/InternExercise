using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace myWebApi.Enity
{
    [Table("User")]
    public class User 
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? SchoolName { get; set; }
        public bool? Gender { get; set; }
        public string? BirthDate { get; set; }
       
    }
}
