using BookStore.DTO;
using BookStore.Services;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoryDTOs = categories.Select(c => new CategoryDTO
            {
                Name = c.Name,
            });
            return Ok(categoryDTOs);
        }

        [Authorize(Roles = "User")]
        [HttpGet("WithBooks/{id:int}")]
        public async Task<ActionResult<CategoryWithBooksDTO>> GetCategoryWithBooks(int id)
        {
            var category = await _categoryService.GetCategoryWithBooksAsync(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            var dto = new CategoryWithBooksDTO
            {
                Name = category.Name,
                Books = category.Books?.Select(b => new BOOkDTO
                {
                    Title = b.Title,
                    Author = b.Author,
                    PageCount = b.PageCount,
                    Price = b.Price
                }).ToList() ?? new List<BOOkDTO>()
            };
            return Ok(dto);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            var categoryDto = new CategoryDTO
            {
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCategory = new Category
            {
                Name = categoryDto.Name
            };

            await _categoryService.AddAsync(newCategory);
            await _categoryService.SaveAsync();

            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var existingCategory = await _categoryService.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound($"Category with ID {id} not found.");

            existingCategory.Name = categoryDto.Name;
            _categoryService.Update(existingCategory);
            await _categoryService.SaveAsync();

            return Ok(categoryDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            _categoryService.DeleteById(id);
            await _categoryService.SaveAsync();

            return Ok("Category deleted successfully.");
        }
    }
}
