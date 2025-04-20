using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Reporisatory
{
    public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
    {
        private readonly BookStoreContext _context;

        public CategoryRepo(BookStoreContext context) : base(context)
        {
            _context = context;
        }

        public Category GetCategoryWithBooks(int id)
        {
            return _context.Categories
                           .Include(c => c.Books)
                           .FirstOrDefault(c => c.Id == id);
        }

    }
}
