using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BackendForFrontend.API.Dtos
{
    [ExcludeFromCodeCoverage]
    public class UserForRegistrationDto
    {
        [Required] 
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required] 
        public BffAddressForCreateDto Address { get; set; }
    }
}
