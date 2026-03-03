using System.Collections.Generic;
using AutoMapper;
using BackendForFrontend.API.Entities;
using Ordering.Contracts.Dtos;

namespace BackendForFrontend.API.Services.OrdersManagement
{
    public class OrdersService : IOrdersService
    {
        private readonly IMapper _mapper;

        public OrdersService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public NormalizedOrderForCreationDto NormalizeOrder(AppUser user, string paymentTypeId, IEnumerable<ProductManagement.Contracts.Dtos.ProductForGetDto> productForGetDtoList)
        {
            var normalizedOrder = new NormalizedOrderForCreationDto
            {
                PaymentTypeId = paymentTypeId,
                Address = _mapper.Map<NormalizedAddressForOrderCreationDto>(user.Address),
                User = _mapper.Map<NormalizedUserForOrderCreationDto>(user),
                Products = _mapper.Map<IEnumerable<NormalizedProductForOrderCreationDto>>(productForGetDtoList)
            };

            return normalizedOrder;
        }
    }
}
