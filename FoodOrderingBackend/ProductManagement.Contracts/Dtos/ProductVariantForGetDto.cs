using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProductVariantForGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SalePercentage { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }
    }
}
