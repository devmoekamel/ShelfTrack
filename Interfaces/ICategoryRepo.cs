using BookStore.Models;

namespace BookStore.Interfaces
{
    public interface ICategoryRepo :IGenericRepo<Category>
    {
        Category GetCategoryWithBooks(int id);
    }
}
