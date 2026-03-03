using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Contracts;
using Ordering.API.Entities;
using Ordering.API.Helpers;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services.Interfaces;

namespace Ordering.API.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IStatusRepository _statusRepository;

        public OrdersService(IOrdersRepository ordersRepository, IStatusRepository statusRepository)
        {
            _ordersRepository = ordersRepository;
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _ordersRepository.GetAllAsync();
        }
        public async Task<Order> GetAsync(string id)
        {
            return await _ordersRepository.GetAsync(id);
        }

        public async Task<Result<Order>> CreateAsync(Order order)
        {
            var initialOrderProcessingResult = await ProcessInitialValuesForOrder(order);

            if (initialOrderProcessingResult.IsFailure)
                return initialOrderProcessingResult;

            return await _ordersRepository.CreateAsync(initialOrderProcessingResult.Value);
        }


        private async Task<Result<Order>> ProcessInitialValuesForOrder(Order order)
        {
            var initialStatus = await _statusRepository.GetByName(StatusNames.Completed);
            if (initialStatus == null)
                return Result<Order>.Fail("Something went wrong. Could not find initial order status in Ordering.API db.");

            order.StatusId = initialStatus.Id;
            order.CreatedOn = DateTime.Now;
            order.CompletedOn = DateTime.Now;
            order.TotalPrice = order.Products.Sum(product => product.Price);

            return Result<Order>.Ok(order);
        }
    }
}
