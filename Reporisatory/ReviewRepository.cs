using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class ReviewRepository : GenericRepo<Review>, IReviewRepository
    {
        public ReviewRepository(BookStoreContext _context) : base(_context)
        {
        }
    }
}
