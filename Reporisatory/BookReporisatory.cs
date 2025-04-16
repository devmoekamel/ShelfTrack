using BookStore.Models;

namespace BookStore.Reporisatory
{
    public class BookReporisatory : GenericRepo<Book>, IBookReporisatory
    {
        private readonly BookStoreContext context;

        public BookReporisatory(BookStoreContext context) : base(context)
        {
            this.context = context;
        }

        public List<Book> GetFreeBooks()
        {
            return context.Books.Where(b => b.IsFree).ToList();
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            return context.Books.Where(b => b.Genre == genre).ToList();
        }


    }
}
