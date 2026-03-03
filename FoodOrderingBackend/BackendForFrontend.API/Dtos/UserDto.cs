using System.Diagnostics.CodeAnalysis;

namespace BackendForFrontend.API.Dtos
{
    [ExcludeFromCodeCoverage]
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
