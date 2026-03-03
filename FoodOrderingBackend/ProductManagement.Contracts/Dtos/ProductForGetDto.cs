using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProductForGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public ProductCategoryDto Category { get; set; }
        public ICollection<ProductVariantForGetDto> Variants { get; set; }
        public WeightTypeDto WeightType { get; set; }
        public ImageForGetDto Image { get; set; }
    }
}
    