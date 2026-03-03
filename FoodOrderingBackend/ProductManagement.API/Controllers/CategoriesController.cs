using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.Entities;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;


namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;
        private readonly ICategoryValidator _categoryValidator;

        public CategoriesController(ICategoriesService categoriesService, IMapper mapper, ICategoryValidator categoryValidator)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
            _categoryValidator = categoryValidator;
        }

        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoriesService.GetAsync(id);

            if (category == null)
                return NotFound($"No category with ID {id} has been found");

            return Ok(_mapper.Map<ProductCategoryForGetDto>(category));
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCategoryForListDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoriesService.GetAllAsync();

            if (!categories.Any())
                return NoContent();

            var categoriesForReturn = _mapper.Map<IEnumerable<ProductCategoryForListDto>>(categories);

            return Ok(categoriesForReturn);
        }



        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryWithProductsDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllWithProducts()
        {
            var categories = await _categoriesService.GetAllCategoriesWithProductsAsync();

            if (!categories.Any())
                return NoContent();

            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductCategoryForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProductCategoryForCreateOrUpdateDto productCategoryForCreateOrUpdateDto)
        {
            var validationResult = _categoryValidator.ValidateCategory(productCategoryForCreateOrUpdateDto);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            var category = _mapper.Map<ProductCategory>(productCategoryForCreateOrUpdateDto);

            var result = await _categoriesService.CreateCategoryAsync(category);

            if(result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Could not create category: {result.Error}");

            var categoryForReturn = _mapper.Map<ProductCategoryForGetDto>(result.Value);

            return CreatedAtRoute("GetCategory", new {id = category.Id}, categoryForReturn);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ProductCategoryForCreateOrUpdateDto categoryForUpdateDto)
        {
            var validationResult = _categoryValidator.ValidateCategory(categoryForUpdateDto);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            var categoryFromRepo = await _categoriesService.GetAsync(id);

            if (categoryFromRepo is null)
                return NotFound($"Cannot find category with id {id}");

            var result = await _categoriesService.UpdateCategoryAsync(categoryFromRepo, categoryForUpdateDto);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Could not update category: {result.Error}");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) 
        {
            var category = await _categoriesService.GetAsync(id);

            if (category is null)
                return NotFound($"Cannot find category with id {id}");

            var result = await _categoriesService.DeleteCategoryAsync(category);
            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Could not delete category: {result.Error}");

            return NoContent();
        }
    }
}   
    