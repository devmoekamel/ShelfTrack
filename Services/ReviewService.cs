using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IReviewService : IEntityService<Review>
    {
    }

    public class ReviewService : EntityService<Review>, IReviewService
    {
        public ReviewService(IGenericRepository<Review> repository) : base(repository)
        {
        }
    }
}
