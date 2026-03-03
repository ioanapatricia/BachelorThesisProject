using System.Threading.Tasks;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.API.Services.BackendForFrontend
{
    public class UsersService : IUsersService
    {
        private readonly DataContext _dataContext;

        public UsersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<AppUser> GetUser(int id)
            => await _dataContext.Users
                .Include(user => user.Address)
                .FirstOrDefaultAsync(user => user.Id == id);
    }
}
    