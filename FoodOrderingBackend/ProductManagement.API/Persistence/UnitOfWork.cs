using System;
using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context,
            IProductRepository products, 
            IImageRepository images,
            IProductCategoryRepository categories,
            IProductVariantRepository variants,
            IWeightTypeRepository weightTypes)
        {
            _context = context;
            Products = products;
            Images = images;
            Categories = categories;
            Variants = variants;
            WeightTypes = weightTypes;
        }

        public IProductRepository Products { get; set; }
        public IImageRepository Images { get; set; }
        public IProductCategoryRepository Categories { get; }
        public IProductVariantRepository Variants { get; }
        public IWeightTypeRepository WeightTypes { get; }


        public async Task<Result> CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception e)
            {
                return Result.Fail($"Error Message: {e.Message}, Inner Exception: {e.InnerException}");
            }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
