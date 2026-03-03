using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence.Repositories
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context) 
            : base(context)
        {
            _context = context;
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await _context.Products
                .Include(prod => prod.Category)
                .Include(prod => prod.Variants)
                .Include(prod => prod.Image)
                .Include(prod => prod.WeightType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(prod => prod.Category)
                .Include(prod => prod.Variants)
                .Include(prod => prod.Image)
                .Include(prod => prod.WeightType)
                .ToListAsync();
        }



        public async Task<IEnumerable<Product>> GetAllByFilterAsync(IEnumerable<ProductFilter> productFilters)
        {
            var result = new List<Product>();

            foreach (var productFilter in productFilters)
            {
                var product = await _context.Products
                    .Include(prod => prod.Category)
                    .Include(prod => prod.Variants)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(prod => prod.Id == productFilter.ProductId);

                if (product == null)
                    return new List<Product>();

                var variants = product.Variants.ToList();
                variants.RemoveAll(variant => variant.Id != productFilter.VariantId);
                product.Variants = variants;

                result.Add(product);
            }

            return result;
        }

        public async Task<IEnumerable<Product>> GetAllProductsByCategoryAsync(int id)
        {
            return await _context.Products
                .Include(prod => prod.Variants)
                .Include(prod => prod.Image)
                .Where(prod => prod.Category.Id == id)
                .ToListAsync();
        }
    }
}
