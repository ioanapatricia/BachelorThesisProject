using System.Linq;
using AutoMapper;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Entities;
using Ordering.Contracts.Dtos;

namespace BackendForFrontend.API.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<UserForRegistrationDto, AppUser>();
            CreateMap<BffAddressForCreateDto, Address>();
            CreateMap<Address, NormalizedAddressForOrderCreationDto>();
            CreateMap<AppUser, NormalizedUserForOrderCreationDto>()
                .ForMember(dest => dest.ExternalId, opts =>
                {
                    opts.MapFrom(src => src.Id);
                });
            CreateMap<ProductManagement.Contracts.Dtos.ProductForGetDto, NormalizedProductForOrderCreationDto>()
                .ForMember(dest => dest.Name, opts =>
                {
                    opts.MapFrom(src => $"{src.Name} {src.Variants.First().Name}");
                })
                .ForMember(dest => dest.Price, opts =>
                {
                    opts.MapFrom(src => src.Variants.First().Price);
                })
                .ForMember(dest => dest.Weight, opts =>
                {
                    opts.MapFrom(src => src.Variants.First().Weight);
                })
                .ForMember(dest => dest.SalePercentage, opts =>
                {
                    opts.MapFrom(src => src.Variants.First().SalePercentage);
                })
                .ForMember(dest => dest.ExternalId, opts =>
                {
                    opts.MapFrom(src => src.Variants.First().Id);
                });


            CreateMap<EmployeeForCreateOrUpdateDto, AppUser>();
            CreateMap<EmployeeAddressDto, Address>();
            CreateMap<AppRole, EmployeeRoleDto>();
        }
    }
}
