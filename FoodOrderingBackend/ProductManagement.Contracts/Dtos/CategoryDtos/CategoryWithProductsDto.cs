using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos.CategoryDtos
{
    [ExcludeFromCodeCoverage]
    public class CategoryWithProductsDto
    {
        public string Name { get; set; }
        public int SortingOrderOnWebpage { get; set; }
        public ImageForGetDto Logo { get; set; }
        public ImageForGetDto Banner { get; set; }
        public IEnumerable<ProductForGetDto> Products { get; set; }  
    }
}
            