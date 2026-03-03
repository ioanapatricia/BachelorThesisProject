using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class StatusForGetDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
