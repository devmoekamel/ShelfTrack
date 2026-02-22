using System.Linq;
using System.Threading.Tasks;
using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IBookService : IEntityService<Book>
    {
        Task<IEnumerable<Book>> GetFreeBooksAsync();
        Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    }

    public class BookService : EntityService<Book>, IBookService
    {
        private readonly IGenericRepository<Book> _repository;

        public BookService(IGenericRepository<Book> repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> GetFreeBooksAsync()
        {
            var spec = new FreeBooksSpecification();
            return await _repository.GetAllWithSpecAsync(spec);
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
        {
            var spec = new BooksByGenreSpecification(genre);
            return await _repository.GetAllWithSpecAsync(spec);
        }
    }

    public class FreeBooksSpecification : BaseSpecification<Book>
    {
        public FreeBooksSpecification()
        {
            Criteria = b => b.IsFree && !b.IsDeleted;
        }
    }

    public class BooksByGenreSpecification : BaseSpecification<Book>
    {
        public BooksByGenreSpecification(string genre)
        {
            Criteria = b => b.Genre == genre && !b.IsDeleted;
        }
    }
}
