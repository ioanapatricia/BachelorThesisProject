using System;
using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services.Interfaces;

namespace ProductManagement.API.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImagesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Image> GetImageAsync(int id)
        {
            return await _unitOfWork.Images.GetAsync(id);
        }

        public async Task<Result<Image>> CreateImageAsync(Image image)
        {
            image = GetProcessedImageWithExtension(image);
            _unitOfWork.Images.Add(image);

            var imageCreationResult = await _unitOfWork.CompleteAsync();

            return imageCreationResult.IsFailure 
                ? Result<Image>.Fail(imageCreationResult.Error) 
                : Result<Image>.Ok(image);
        }

        public async Task<string> GetImageAsBase64String(int id)
        {
            var image = await _unitOfWork.Images.GetAsync(id);

            return image is null 
                ? null 
                : Convert.ToBase64String(image.Data);
        }

        public Image GetProcessedImageWithExtension(Image image)
        {
            return new()
            {
                Id = image.Id,
                Data = image.Data,
                Extension = image.Name.Substring(image.Name.LastIndexOf('.') + 1),
                Name = image.Name.Split('.')[0]
            };
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _unitOfWork.Images.GetAsync(id);
            _unitOfWork.Images.Remove(image);
        }
    }
}
    