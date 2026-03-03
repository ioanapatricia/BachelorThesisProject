using System.Diagnostics.CodeAnalysis;

namespace BackendForFrontend.API.Dtos
{
    [ExcludeFromCodeCoverage]
    public class BffProductForOrderCreationDto
    {
        public int Id { get; set; }
        public int VariantId { get; set; }
    }
}
    