using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Entities;
using Ordering.API.Helpers;
using Ordering.API.Services.Interfaces;
using Ordering.Contracts.Dtos;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IPaymentTypesService _paymentTypesService;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersService ordersService, IPaymentTypesService paymentTypesService, IMapper mapper)
        {
            _ordersService = ordersService;
            _paymentTypesService = paymentTypesService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _ordersService.GetAllAsync();

            if (!orders.Any())
                return NoContent();

            var orderForReturn = _mapper.Map<IEnumerable<OrderForGetDto>>(orders);

            return Ok(orderForReturn);
        }

        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            if (!RenameMe.IsAValid24HexString(id))
                return BadRequest("This id is not a valid 24 hex string");

            var result  = await _ordersService.GetAsync(id);

            if (result is null)
                return NotFound("The order you're looking for does not exist");

            var orderForReturn = _mapper.Map<OrderForGetDto>(result);
                
            return Ok(orderForReturn);
        }   

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(NormalizedOrderForCreationDto orderForCreateDto)
        {
            if (!RenameMe.IsAValid24HexString(orderForCreateDto.PaymentTypeId))
                return BadRequest("This payment type id is not a valid 24 hex string");

            if (!await _paymentTypesService.ExistsAsync(orderForCreateDto.PaymentTypeId))
                return BadRequest("This payment type does not exist");

            var order = _mapper.Map<Order>(orderForCreateDto);

            var result = await _ordersService.CreateAsync(order);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

            var orderForReturn = _mapper.Map<OrderForGetDto>(result.Value);

            return CreatedAtRoute("GetOrder",
                new { id = orderForReturn.Id }, orderForReturn);
        }
    }
}
        