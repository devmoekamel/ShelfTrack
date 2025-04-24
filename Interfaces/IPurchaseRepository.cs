using BookStore.Models;

namespace BookStore.Interfaces
{
    public interface IPurchaseRepository:IGenericRepo<Purchase>
    {
        IQueryable<Purchase> GetAll();
    }
}
