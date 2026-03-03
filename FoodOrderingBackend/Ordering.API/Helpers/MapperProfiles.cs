using AutoMapper;
using Ordering.API.Entities;
using Ordering.Contracts.Dtos;

namespace Ordering.API.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Address, AddressForReturnDto>();
            CreateMap<PaymentType, PaymentTypeForGetDto>();
            CreateMap<Status, StatusForGetDto>();
            CreateMap<User, UserForGetDto>();
            CreateMap<Product, ProductForGetDto>();
            CreateMap<Order, OrderForGetDto>();


            CreateMap<NormalizedAddressForOrderCreationDto, Address>();
            CreateMap<NormalizedUserForOrderCreationDto, User>();
            CreateMap<NormalizedProductForOrderCreationDto, Product>();
            CreateMap<NormalizedOrderForCreationDto, Order>();
        }
    }
}
