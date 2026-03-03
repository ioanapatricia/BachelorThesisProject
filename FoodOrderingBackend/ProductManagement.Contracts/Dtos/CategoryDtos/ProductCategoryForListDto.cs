using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos.CategoryDtos
{
    [ExcludeFromCodeCoverage]
    public class ProductCategoryForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ImageForGetDto Logo { get; set; }
    }
}
    