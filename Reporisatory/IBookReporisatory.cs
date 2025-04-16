using BookStore.Interfaces;
using BookStore.Models;

namespace BookStore.Reporisatory
{
    public interface IBookReporisatory : IGenericRepo<Book>
    {
        List<Book> GetFreeBooks();
        List<Book> GetBooksByGenre(string genre);

    }
}
