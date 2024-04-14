using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
