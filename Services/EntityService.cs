using System.Linq.Expressions;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IEntityService<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteById(int id);
        Task SaveAsync();
    }

    public class EntityService<T> : IEntityService<T> where T : BaseModel
    {
        private readonly IGenericRepository<T> _repository;

        public EntityService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await _repository.GetByIdWithSpecAsync(spec);
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await _repository.GetAllWithSpecAsync(spec);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await _repository.GetCountAsync(spec);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public void DeleteById(int id)
        {
            _repository.DeleteById(id);
        }

        public async Task SaveAsync()
        {
            await _repository.SaveAsync();
        }
    }
}
