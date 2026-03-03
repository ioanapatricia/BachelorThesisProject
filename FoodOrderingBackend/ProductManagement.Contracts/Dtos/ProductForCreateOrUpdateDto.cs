using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ProductForCreateOrUpdateDto
    {
        public string Name { get; set; }    
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public int? WeightTypeId { get; set; }  
        public ImageForCreateDto Image { get; set; }
        public ICollection<ProductVariantForCreateDto> Variants { get; set; }
    }
}
