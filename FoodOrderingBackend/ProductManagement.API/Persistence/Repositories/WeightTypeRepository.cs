using ProductManagement.API.Entities;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence.Repositories
{
    public class WeightTypeRepository : Repository<WeightType>, IWeightTypeRepository
    {
        private readonly DataContext _context;
        public WeightTypeRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
