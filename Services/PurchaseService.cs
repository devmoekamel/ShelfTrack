using BookStore.Infrastructure;
using BookStore.Models;

namespace BookStore.Services
{
    public interface IPurchaseService : IEntityService<Purchase>
    {
    }

    public class PurchaseService : EntityService<Purchase>, IPurchaseService
    {
        public PurchaseService(IGenericRepository<Purchase> repository) : base(repository)
        {
        }
    }
}
