using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProductVariantForCreateDto
    {
        public string Name { get; set; }
        public int? SalePercentage { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }
    }
}
