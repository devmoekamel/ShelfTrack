using BookStore.context;
using BookStore.Interfaces;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Reporisatory
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseModel
    {
        BookStoreContext context;
        public GenericRepo(BookStoreContext _context)
        {
            context = _context;

        }
        public void Add(T obj)
        {
            context.Set<T>().Add(obj);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public T GetById(int Id)
        {
            return context.Set<T>().FirstOrDefault(x => x.Id==Id);
        }

        public void RemoveByObj(T obj)
        {
            context.Set<T>().Remove(obj);
        }
        public void RemoveById(int id)
        {
            var existingEntity = GetById(id);
            existingEntity.IsDeleted = false;
        }

        public void Update(int id, T obj)
        {
            var existingEntity = GetById(id);
            if (existingEntity != null)
            {
                context.Set<T>().Update(existingEntity);
            }
        }
        public void Save()

        {
            context.SaveChanges();
        }

    }
}
