
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.context
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Plan> Plans { get; set; }
        public DbSet<Mission> Missions { get; set; }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Scientific" },
                new Category { Id = 3, Name = "History" }
            );


            modelBuilder.Entity<Book>().HasData(
                    new Book
                    {
                        Id = 1,
                        Title = "The Great Gatsby",
                        Author = "F. Scott Fitzgerald",
                        Description = "A novel set in the Roaring Twenties.",
                        Genre = "Novel",
                        ISBN = "9780743273565",
                        PageCount = 180,
                        Price = 9.99,
                        CoverImageURL = "https://example.com/gatsby.jpg",
                        PublishedDate = new DateTime(1925, 4, 10),
                        IsFree = false,
                        Categoryid = 1 // Fiction
                    },
                     new Book
                     {
                         Id = 2,
                         Title = "The Great Gatsby",
                         Author = "F. Scott Fitzgerald",
                         Description = "A novel set in the Roaring Twenties.",
                         Genre = "Novel",
                         ISBN = "9780743273565",
                         PageCount = 180,
                         Price = 9.99,
                         CoverImageURL = "https://example.com/gatsby.jpg",
                         PublishedDate = new DateTime(1925, 4, 10),
                         IsFree = false,
                         Categoryid = 2 // Fiction
                     },
                      new Book
                      {
                          Id=3,
                          Title = "The Great Gatsby",
                          Author = "F. Scott Fitzgerald",
                          Description = "A novel set in the Roaring Twenties.",
                          Genre = "Novel",
                          ISBN = "9780743273565",
                          PageCount = 180,
                          Price = 9.99,
                          CoverImageURL = "https://example.com/gatsby.jpg",
                          PublishedDate = new DateTime(1925, 4, 10),
                          IsFree = false,
                          Categoryid = 3 // Fiction
                      }
                    );

        }
    }
}
