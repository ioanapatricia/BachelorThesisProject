using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class NormalizedUserForOrderCreationDto
    {
        public int ExternalId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }    
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
    }
}
