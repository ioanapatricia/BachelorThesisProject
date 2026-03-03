using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IProductValidator _productValidator;
        private readonly IMapper _mapper;

        public ProductsController(IProductsService productsService, IProductValidator productValidator , IMapper mapper)
        {
            _productsService = productsService;
            _productValidator = productValidator;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productsService.GetProductAsync(id);

            if (product == null)
                return NotFound($"No product with ID {id} has been found");

            return Ok(_mapper.Map<ProductForGetDto>(product));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productsService.GetProductsAsync();

            if (!products.Any())
                return NoContent();

            return Ok(_mapper.Map<IEnumerable<ProductForGetDto>>(products));
        }


        [HttpGet("filtered")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetProductsByFilter(IEnumerable<ProductFilter> productFilters)
        {
            var result = await _productsService.GetProductsByFilterAsync(productFilters);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, "Broken entries in PM.API");

            if (!result.Value.Any())
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<ProductForGetDto>>(result.Value));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProductForCreateOrUpdateDto productForCreateDto)
        {
            var validationResult = await _productValidator.ValidateProduct(productForCreateDto);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            var product = _mapper.Map<Product>(productForCreateDto);

            var result = await _productsService.CreateProductAsync(product);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Could not create product: {result.Error}");

            var productForReturn = _mapper.Map<ProductForGetDto>(result.Value);

            return CreatedAtRoute("GetProduct",
                new { id = product.Id }, productForReturn);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ProductForCreateOrUpdateDto productForUpdateDto)
        {
            var validationResult = await _productValidator.ValidateProduct(productForUpdateDto);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            var productFromRepo = await _productsService.GetProductAsync(id);

            if (productFromRepo == null)
                return NotFound($"Cannot find product with id {id}");

            var result = await _productsService.UpdateProductAsync(productFromRepo, productForUpdateDto);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Could not update product: {result.Error}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productsService.GetProductAsync(id);
            if (product == null)
                return NotFound("The product does not exist.");

            var result = await _productsService.DeleteProductAsync(product);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error encountered while trying to delete the product from the database: {result.Error}");

            return NoContent();
        }
    }
}
