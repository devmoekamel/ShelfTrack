
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BookStore.Models
{
    public class BookStoreContext: DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Book> Category { get; set; }
       
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Mission> Missions { get; set; }

        public DbSet<Purchase> Purchases{ get; set; }
        public DbSet<Review> Reviews { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options)
    : base(options)
        {
        }

    }
}
