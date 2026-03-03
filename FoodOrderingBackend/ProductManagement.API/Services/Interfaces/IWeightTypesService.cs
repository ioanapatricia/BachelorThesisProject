using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement.API.Entities;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IWeightTypesService
    {
        Task<IEnumerable<WeightType>> GetWeightTypesAsync();
    }
}
