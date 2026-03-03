using System;
using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IImageRepository Images { get; }
        IProductCategoryRepository Categories { get; }
        IProductVariantRepository Variants { get; }
        IWeightTypeRepository WeightTypes { get; }


        Task<Result> CompleteAsync();
    }
}
            