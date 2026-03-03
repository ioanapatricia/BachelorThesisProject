namespace ProductManagement.API.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortingOrderOnWebpage { get; set; }
        public int? LogoId { get; set; }
        public Image Logo { get; set; }
        public int? BannerId { get; set; }
        public Image Banner { get; set; }
    }
}
                