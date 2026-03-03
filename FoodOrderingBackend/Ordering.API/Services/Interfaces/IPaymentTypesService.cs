using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;

namespace Ordering.API.Services.Interfaces
{
    public interface IPaymentTypesService
    {
        Task<IEnumerable<PaymentType>> GetAllAsync();
        Task<bool> ExistsAsync(string id);   
    }
}
