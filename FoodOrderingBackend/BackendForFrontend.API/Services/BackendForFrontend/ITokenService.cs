using System.Threading.Tasks;
using BackendForFrontend.API.Entities;

namespace BackendForFrontend.API.Services.BackendForFrontend
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
