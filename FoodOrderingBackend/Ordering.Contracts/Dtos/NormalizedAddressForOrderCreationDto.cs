using System.Diagnostics.CodeAnalysis;

namespace Ordering.Contracts.Dtos
{
    [ExcludeFromCodeCoverage]
    public class NormalizedAddressForOrderCreationDto
    {
        public string City { get; set; }
        public string County { get; set; }
        public string Sector { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string Building { get; set; }
        public int? Floor { get; set; } 
        public string Entrance { get; set; }
        public int? ApartmentNumber { get; set; }
    }
}
