using BookShopApi.Dtos.Category;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllCategorysAsync();
            var categoriesDto = categories.Select(c => c.ToCategoryDto($"{Request.Scheme}://{Request.Host}{c.ImageUrl}"));
            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryByIdAsync([FromRoute] int id)
        {
            var category = await _categoryRepo.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound(new { errorMessage = $"The category with id: {id}, Not Found." });

            return Ok(category.ToCategoryDto($"{Request.Scheme}://{Request.Host}{category.ImageUrl}"));
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategoryAsync([FromForm] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCategory = await _categoryRepo.CreateCategoryAsync(categoryDto);
            return Created($"api/category/{createdCategory.Id}", createdCategory.ToCategoryDto());
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCategoryAsync([FromForm] UpdateCategoryDto categoryDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _categoryRepo.UpdateCategoryAsync(categoryDto, id);

            if (category == null)
                return NotFound(new { errorMessage = $"The category with id: {id}, Not Found." });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int id)
        {
            var category = await _categoryRepo.DeleteCategoryAsync(id);

            if (category == null)
                return NotFound(new { errorMessage = $"The category with id: {id}, Not Found." });

            return NoContent();
        }
    }
}
