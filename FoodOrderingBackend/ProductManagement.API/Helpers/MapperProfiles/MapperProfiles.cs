using AutoMapper;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.API.Helpers.MapperProfiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<ProductForCreateOrUpdateDto, Product>();
            CreateMap<ProductVariantForCreateDto, ProductVariant>();
            CreateMap<ImageForCreateDto, Image>();


            CreateMap<Product, ProductForGetDto>();
            CreateMap<Image, ImageForGetDto>();
            CreateMap<ProductVariant, ProductVariantForGetDto>();

            CreateMap<ProductCategoryForCreateOrUpdateDto, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryForListDto>();
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategory, ProductCategoryForGetDto>();

            CreateMap<WeightType, WeightTypeDto>();

            CreateMap<string, byte[]>()
                .ConvertUsing<Base64StringToByteArrayConverter>();
        }
    }
}