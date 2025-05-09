using System.ComponentModel.DataAnnotations;

namespace myWebApi.Model.Response
{
    public class UserReponse
    {
        public string Name { get; set; }
      
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? SchoolName { get; set; }
        public bool? Gender { get; set; }
        public string? BirthDate { get; set; }
    }
}
