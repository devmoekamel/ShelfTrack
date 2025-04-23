using BookStore.Models;

namespace BookStore.Interfaces
{
    public interface IReviewRepository:IGenericRepo<Review>
    {
        IQueryable<Review> GetAll();

    }
}
