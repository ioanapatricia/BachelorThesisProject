using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Models
{
    [ExcludeFromCodeCoverage]
    public class ProductFilter
    {
        public int ProductId { get; set; }
        public int VariantId { get; set; }
    }
}
