using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;

namespace Ordering.API.Persistence
{
    public class PaymentTypesRepository : IPaymentTypesRepository
    {
        private readonly IMongoCollection<PaymentType> _paymentTypeCollection;

        public PaymentTypesRepository(IMongoClient mongoClient)
        {
            _paymentTypeCollection = mongoClient.GetDatabase("OrdersDb").GetCollection<PaymentType>("PaymentTypes");
        }
        public async Task<IEnumerable<PaymentType>> GetAllAsync()
        {
            return await _paymentTypeCollection
                .Find(Builders<PaymentType>.Filter.Empty)
                .ToListAsync();
        }

        public async Task<PaymentType> Get(string id)
        {
            return await _paymentTypeCollection
                .Find(paymentType => paymentType.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
    