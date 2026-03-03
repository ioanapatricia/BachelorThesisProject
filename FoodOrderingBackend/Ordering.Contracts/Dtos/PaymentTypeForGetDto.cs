using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class PaymentTypeForGetDto
    {
        public string Id { get; set; }  
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
    