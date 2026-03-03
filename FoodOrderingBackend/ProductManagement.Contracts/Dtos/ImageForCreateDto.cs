using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ImageForCreateDto
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
