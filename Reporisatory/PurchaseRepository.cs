using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class PurchaseRepository : GenericRepo<Purchase>, IPurchaseRepository
    {
        BookStoreContext context;
        public PurchaseRepository(BookStoreContext _context) : base(_context)
        {
            context = _context;
        }

        public IQueryable<Purchase> GetAll()
        {
            return context.Purchases;
        }
    }
}
