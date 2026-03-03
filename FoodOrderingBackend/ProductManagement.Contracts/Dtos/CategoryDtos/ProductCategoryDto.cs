using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos.CategoryDtos
{
    [ExcludeFromCodeCoverage]
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
    