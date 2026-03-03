using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;

namespace Ordering.API.Persistence
{
    public class StatusRepository : IStatusRepository
    {
        private readonly IMongoCollection<Status> _statusCollection;
        public StatusRepository(IMongoClient mongoClient)
        {
            _statusCollection = mongoClient.GetDatabase("OrdersDb").GetCollection<Status>("Status");
        }

        public async Task<IEnumerable<Status>> GetAllAsync()
        {
            return await _statusCollection
                .Find(Builders<Status>.Filter.Empty)
                .ToListAsync();
        }


        public async Task<Status> GetByName(string name)
        {
            return await _statusCollection
                .Find(status => status.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
    