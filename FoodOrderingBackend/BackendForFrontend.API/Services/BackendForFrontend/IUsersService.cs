using System.Threading.Tasks;
using BackendForFrontend.API.Entities;

namespace BackendForFrontend.API.Services.BackendForFrontend
{
    public interface IUsersService
    {
        Task<AppUser> GetUser(int id);
    }
}
    