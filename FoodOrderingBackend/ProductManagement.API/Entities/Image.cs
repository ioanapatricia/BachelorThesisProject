namespace ProductManagement.API.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
    