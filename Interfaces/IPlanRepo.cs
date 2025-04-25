using BookStore.Models;

namespace BookStore.Interfaces
{
    public interface IPlanRepo:IGenericRepo<Plan>
    {
      
       Plan GetplanByBookId(int bookId);
    }
}
