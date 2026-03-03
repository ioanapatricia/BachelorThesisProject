using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ImageForGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }   
    }
}
