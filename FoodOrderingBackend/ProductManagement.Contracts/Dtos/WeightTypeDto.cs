using System.Diagnostics.CodeAnalysis;

namespace ProductManagement.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class WeightTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
