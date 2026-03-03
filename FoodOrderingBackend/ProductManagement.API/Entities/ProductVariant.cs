namespace ProductManagement.API.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public float Weight { get; set; }
        public int? SalePercentage { get; set; }
        public int? ProductId { get; set; } 
        public Product Product { get; set; }
    }
}
