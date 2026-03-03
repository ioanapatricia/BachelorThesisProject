using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class NormalizedProductForOrderCreationDto
    {
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int? SalePercentage { get; set; }
        public decimal Price { get; set; }  
        public float Weight { get; set; }
    }
}
