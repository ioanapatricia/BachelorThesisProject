using System.Collections.Generic;
using BackendForFrontend.API.Entities;
using Ordering.Contracts.Dtos;

namespace BackendForFrontend.API.Services.OrdersManagement
{
    public interface IOrdersService
    {   
        NormalizedOrderForCreationDto NormalizeOrder(AppUser user, string paymentTypeId, IEnumerable<ProductManagement.Contracts.Dtos.ProductForGetDto> productForGetDtoList);
    }
}
    