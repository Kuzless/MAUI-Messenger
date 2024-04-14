using MyMessenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using LinqKit;

namespace MyMessenger.Domain.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DatabaseContext dbContext;
        public GenericRepository(DatabaseContext context)
        {
            dbContext = context;
        }
        public async Task<T> GetById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }
        public IQueryable<T> GetAll()
        {
            return dbContext.Set<T>();
        }
        public async Task<IEnumerable<T>> FilterByQuery(IQueryable<T> query, Dictionary<string, bool>? sort, int skipSize, int pageSize, string subs, string owner = "")
        {
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var type = typeof(T);
            //Filter
            if (!string.IsNullOrEmpty(subs))
            {
                var predicate = PredicateBuilder.False<T>();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        if (!string.IsNullOrEmpty(subs))
                        {
                            predicate = predicate.Or(GetColumnContains<T>(property, subs));
                        }
                    }
                }
                query = query.Where(predicate);
            }
            if (!string.IsNullOrEmpty(owner))
            {
                var predicate = PredicateBuilder.False<T>();

                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        predicate = predicate.Or(GetColumnContains<T>(property, owner));
                    }
                }
                query = query.Where(predicate);
            }

            //Sort
            if (sort != null)
            {
                var parameter = Expression.Parameter(type, "item");
                foreach (var (key, value) in sort)
                {
                    var property = type.GetProperty(key);
                    var sortExpression = Expression.Lambda<Func<T, object>>
                        (Expression.Convert(Expression.Property(parameter, property), typeof(object)), parameter);
                    if (value)
                    {
                        query = query.AsQueryable().OrderByDescending<T, object>(sortExpression);
                    }
                    else
                    {
                        query = query.AsQueryable().OrderBy<T, object>(sortExpression);
                    }
                }
            }

            var orderedQuery = await query.ToListAsync();

            //Paging
            return await query.Skip(skipSize).Take(pageSize).ToListAsync();
        }

        public void Add(T item)
        {
            dbContext.Set<T>().Add(item);
        }

        public void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            dbContext.Set<T>().Remove(item);
        }

        public int GetNumberOfRecords()
        {
            IQueryable<T> query = dbContext.Set<T>();
            return query.Count();
        }
        private static Expression<Func<T, bool>> GetColumnContains<T>(PropertyInfo property, string term)
        {
            var obj = Expression.Parameter(typeof(T), "obj");

            var objProperty = Expression.Property(obj, property);
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var termExpression = Expression.Constant(term);
            var containsCall = Expression.Call(objProperty, containsMethod, termExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, obj);

            return lambda;
        }
    }
}
