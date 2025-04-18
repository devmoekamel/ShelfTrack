using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class PurchaseRepository : GenericRepo<Purchase>, IPurchaseRepository
    {
       
        public PurchaseRepository(BookStoreContext _context) : base(_context)
        {
            
        }
        
       
    }
}
