using Global.Contracts;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Validators.Interfaces
{
    public interface IImageValidator
    {
        Result ValidateImage(ImageForCreateDto imageForCreateDto);
    }
}
