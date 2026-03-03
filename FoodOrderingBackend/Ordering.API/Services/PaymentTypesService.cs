using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services.Interfaces;

namespace Ordering.API.Services
{
    public class PaymentTypesService : IPaymentTypesService
    {
        private readonly IPaymentTypesRepository _paymentTypesRepository;

        public PaymentTypesService(IPaymentTypesRepository paymentTypesRepository)
        {
            _paymentTypesRepository = paymentTypesRepository;
        }
        public async Task<IEnumerable<PaymentType>> GetAllAsync()
        {
            return await _paymentTypesRepository.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _paymentTypesRepository.Get(id) is not null;
        }
    }
}
    