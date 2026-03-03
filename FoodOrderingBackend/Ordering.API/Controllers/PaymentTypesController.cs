using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Services.Interfaces;
using Ordering.Contracts.Dtos;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypesController : ControllerBase
    {
        private readonly IPaymentTypesService _paymentTypesService;
        private readonly IMapper _mapper;

        public PaymentTypesController(IPaymentTypesService paymentTypesService, IMapper mapper)
        {
            _paymentTypesService = paymentTypesService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentTypeForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var paymentTypes = await _paymentTypesService.GetAllAsync();

            if (!paymentTypes.Any())
                return NoContent();

            var paymentTypesForReturn = _mapper.Map<IEnumerable<PaymentTypeForGetDto>>(paymentTypes);

            return Ok(paymentTypesForReturn);
        }
    }
}
