using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface ICategoryService : IEntityService<Category>
    {
        Task<Category?> GetCategoryWithBooksAsync(int id);
    }

    public class CategoryService : EntityService<Category>, ICategoryService
    {
        private readonly IGenericRepository<Category> _repository;

        public CategoryService(IGenericRepository<Category> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Category?> GetCategoryWithBooksAsync(int id)
        {
            var spec = new CategoryWithBooksSpecification();
            spec.Criteria = c => c.Id == id && !c.IsDeleted;
            return await _repository.GetByIdWithSpecAsync(spec);
        }
    }

    public class CategoryWithBooksSpecification : BaseSpecification<Category>
    {
        public CategoryWithBooksSpecification()
        {
            AddInclude(c => c.Books);
        }
    }
}
