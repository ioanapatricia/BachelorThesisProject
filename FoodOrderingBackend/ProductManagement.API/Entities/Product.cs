using System.Collections.Generic;

namespace ProductManagement.API.Entities
{
    public class Product
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public int? CategoryId { get; set; }
        public ProductCategory Category { get; set; }
        public int? WeightTypeId { get; set; }
        public WeightType WeightType { get; set; }
        public int? ImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<ProductVariant> Variants { get; set; }
    }
}   
    