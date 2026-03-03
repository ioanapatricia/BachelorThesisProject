using System.Diagnostics.CodeAnalysis;

namespace BackendForFrontend.API.Dtos
{
    [ExcludeFromCodeCoverage]
    public class BffAddressForCreateDto
    {
        public string City { get; set; }
        public string County { get; set; }
        public int? Sector { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        public string Building { get; set; }
        public int? Floor { get; set; }
        public string Entrance { get; set; }
        public int? ApartmentNumber { get; set; }
    }
}

