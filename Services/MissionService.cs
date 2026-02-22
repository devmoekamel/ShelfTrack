using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IMissionService : IEntityService<Mission>
    {
        Task<IEnumerable<Mission>> GetMissionsByPlanIdAsync(int planId);
    }

    public class MissionService : EntityService<Mission>, IMissionService
    {
        private readonly IGenericRepository<Mission> _repository;

        public MissionService(IGenericRepository<Mission> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Mission>> GetMissionsByPlanIdAsync(int planId)
        {
            var spec = new MissionsByPlanIdSpecification();
            spec.Criteria = m => m.PlanId == planId && !m.IsDeleted;
            return await _repository.GetAllWithSpecAsync(spec);
        }
    }

    public class MissionsByPlanIdSpecification : BaseSpecification<Mission>
    {
    }
}
