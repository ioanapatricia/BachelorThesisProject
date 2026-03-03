using ProductManagement.API.Entities;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence.Repositories
{
    public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        private readonly DataContext _context;

        public ProductVariantRepository(DataContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
