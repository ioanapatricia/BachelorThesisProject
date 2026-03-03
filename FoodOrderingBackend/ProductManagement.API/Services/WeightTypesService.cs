using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services.Interfaces;


namespace ProductManagement.API.Services
{
    public class WeightTypesService : IWeightTypesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WeightTypesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<WeightType>> GetWeightTypesAsync()
            => await _unitOfWork.WeightTypes.GetAllAsync();
    }
}
