using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services.Interfaces;

namespace Ordering.API.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }
        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _statusRepository.GetAllAsync();
        }
    }
}
