using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos.CategoryDtos
{
    [ExcludeFromCodeCoverage]
    public class ProductCategoryForCreateOrUpdateDto
    {
        public string Name { get; set; }
        public int SortingOrderOnWebpage { get; set; }
        public ImageForCreateDto Logo { get; set; }
        public ImageForCreateDto Banner { get; set; }
    }
}
