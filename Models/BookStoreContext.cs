
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BookStore.Models
{
    public class BookStoreContext: DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
    : base(options)
        {
        }

    }
}
