using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly DataContext _context;
        public ProductCategoryRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ProductCategory> GetAsync(int id)
        {
            return await _context.ProductCategories
                .Include(cat => cat.Logo)
                .Include(cat => cat.Banner)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllCategoriesWithImagesAsync()
        {
            return await _context.ProductCategories.Include(c => c.Banner).Include(c => c.Logo)
                         .ToListAsync();
        }
    }
}
    