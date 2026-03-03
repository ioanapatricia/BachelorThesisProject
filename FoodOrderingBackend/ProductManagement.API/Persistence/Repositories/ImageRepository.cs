using ProductManagement.API.Entities;
using ProductManagement.API.Persistence.Repositories.Interfaces;

namespace ProductManagement.API.Persistence.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private readonly DataContext _context;
        public ImageRepository(DataContext context) 
            : base(context)
        {
            _context = context;
        }
    }
}
