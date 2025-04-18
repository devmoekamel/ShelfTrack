using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class PlanRepo : GenericRepo<Plan> ,IPlanRepo
    {
        public PlanRepo(BookStoreContext _context) : base(_context)
        {
        }
    }
}
