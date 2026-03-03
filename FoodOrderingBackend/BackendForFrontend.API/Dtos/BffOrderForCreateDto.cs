using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ordering.Contracts.Models;

namespace BackendForFrontend.API.Dtos
{
    [ExcludeFromCodeCoverage]
    public class BffOrderForCreateDto
    {
        public string PaymentTypeId { get; set; }
        public ICollection<ProductFilter> ProductFilters { get; set; }    
    }
}
    