using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // GET: api/Category
        [HttpGet]
        public ActionResult GetAll()
        {
            var categories = _categoryRepo.GetAll().Select(c=>new CategoryDTO()
            {
                Id = c.Id, 
                Name = c.Name,
            });
            return Ok(categories);
        }

        // GET: api/Category/WithBooks/5
        [HttpGet("WithBooks/{id:int}")]
        public ActionResult<CategoryWithBooksDTO> GetCategoryWithBooks(int id)
        {
            var category = _categoryRepo.GetCategoryWithBooks(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            var dto = new CategoryWithBooksDTO
            {
                Id = category.Id,
                Name = category.Name,
                Books = category.Books?.Select(b => new BookDTO
                {
                    Title = b.Title,
                    Author = b.Author,
                    Price = b.Price

                }).ToList() ?? new List<BookDTO>()
            };
            return Ok(dto);
        }

        // GET: api/Category/5
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");
            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        public ActionResult AddCategory(CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newCategory = new Category
            {
                Name = categoryDto.Name
            };

            _categoryRepo.Add(newCategory);
            _categoryRepo.Save();

            return Ok(newCategory);
        }

        // PUT: api/Category/5
        [HttpPut("{id:int}")]
        public ActionResult UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var existingCategory = _categoryRepo.GetById(id);
            if (existingCategory == null)
                return NotFound($"Category with ID {id} not found.");

            existingCategory.Name = categoryDto.Name;
            _categoryRepo.Update(id, existingCategory);
            _categoryRepo.Save();

            return Ok(existingCategory);
        }

        // DELETE: api/Category/5
        [HttpDelete("{id:int}")]
        public ActionResult DeleteCategory(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");

            _categoryRepo.RemoveById(id);
            _categoryRepo.Save();

            return Ok("Category deleted successfully.");
        }
    }
}
