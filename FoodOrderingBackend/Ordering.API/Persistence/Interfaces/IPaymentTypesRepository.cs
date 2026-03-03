using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;

namespace Ordering.API.Persistence.Interfaces
{
    public interface IPaymentTypesRepository
    {
        Task<IEnumerable<PaymentType>> GetAllAsync();

        Task<PaymentType> Get(string id);
    }
}
