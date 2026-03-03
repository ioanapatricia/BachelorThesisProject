using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class NormalizedOrderForCreationDto      
    {
        public string PaymentTypeId { get; set; }
        public NormalizedAddressForOrderCreationDto Address { get; set; }
        public NormalizedUserForOrderCreationDto User { get; set; }   
        public IEnumerable<NormalizedProductForOrderCreationDto> Products { get; set; }
    }
}
    