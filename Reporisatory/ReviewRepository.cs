using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class ReviewRepository : GenericRepo<Review>, IReviewRepository
    {
        BookStoreContext context;
        public ReviewRepository(BookStoreContext _context) : base(_context)
        {
            context = _context;
        }



        public IQueryable<Review> GetAll()
        {
            return context.Reviews; 
        }

    }
}
