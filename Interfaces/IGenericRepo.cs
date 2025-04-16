namespace BookStore.Interfaces
{
    public interface IGenericRepo<T>
    {
        public IEnumerable<T> GetAll();
        public T GetById(int Id);
        public void Add(T obj);
        public void RemoveByObj(T obj);
        public void RemoveById(int id);
        public void Update(int id, T obj);
        public void Save();
    }
}
