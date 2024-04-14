namespace MyMessenger.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(Int32 id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> FilterByQuery(IQueryable<T> query, Dictionary<string, bool>? sort, int skipSize, int pageSize, string subs, string owner = "");
        void Add(T item);
        void Update(T item);
        void Delete(T item);
        int GetNumberOfRecords();
    }
}
