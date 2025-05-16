using BookStore.DTO;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
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
      [Authorize(Roles = "User")]

        [HttpGet]
        public ActionResult GetAll()
        {
            var categories = _categoryRepo.GetAll().Select(c=>new CategoryDTO()
            {
                Name = c.Name,
            });
            return Ok(categories);
        }

        [Authorize(Roles = "User")]

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

        // GET: api/Category/5
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var category = _categoryRepo.GetById(id);
            if (category == null)
                return NotFound($"Category with ID {id} not found.");
           
            var categoryDto = new CategoryDTO
            {
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        // POST: api/Category
        [Authorize(Roles = "Admin")]

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

            return Ok(categoryDto);
        }

        // PUT: api/Category/5
        [Authorize(Roles = "Admin")]

        [HttpPut("{id:int}")]
        public ActionResult UpdateCategory(int id, CategoryDTO categoryDto)
        {
            var existingCategory = _categoryRepo.GetById(id);
            if (existingCategory == null)
                return NotFound($"Category with ID {id} not found.");

            existingCategory.Name = categoryDto.Name;
            _categoryRepo.Update(id, existingCategory);
            _categoryRepo.Save();

            return Ok(categoryDto);
        }

        // DELETE: api/Category/5
        [Authorize(Roles = "Admin")]

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
