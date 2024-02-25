using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.DTOs.User
{
    public class UserRegDTO : UserLoginDTO
    {
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        public string? Role { get; set; }


    }
}