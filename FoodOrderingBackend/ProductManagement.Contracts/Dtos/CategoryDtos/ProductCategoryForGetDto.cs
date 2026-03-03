using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos.CategoryDtos
{
    [ExcludeFromCodeCoverage]
    public class ProductCategoryForGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortingOrderOnWebpage { get; set; }
        public ImageForGetDto Logo { get; set; }
        public ImageForGetDto Banner { get; set; }
    }
}
