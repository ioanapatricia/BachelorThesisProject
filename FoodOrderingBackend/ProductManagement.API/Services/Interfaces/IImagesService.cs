using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Entities;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IImagesService
    {
        Task<Image> GetImageAsync(int id);
        Task<Result<Image>> CreateImageAsync(Image image);

        Task<string> GetImageAsBase64String(int id);
        Image GetProcessedImageWithExtension(Image image);
        Task DeleteAsync(int id);
    }
}
    