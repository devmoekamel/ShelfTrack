using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IPlanService : IEntityService<Plan>
    {
        Task<Plan?> GetPlanByBookIdAsync(int bookId);
    }

    public class PlanService : EntityService<Plan>, IPlanService
    {
        private readonly IGenericRepository<Plan> _repository;

        public PlanService(IGenericRepository<Plan> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Plan?> GetPlanByBookIdAsync(int bookId)
        {
            var spec = new PlanByBookIdSpecification();
            spec.Criteria = p => p.BookId == bookId && !p.IsDeleted;
            return await _repository.GetByIdWithSpecAsync(spec);
        }
    }

    public class PlanByBookIdSpecification : BaseSpecification<Plan>
    {
    }
}
