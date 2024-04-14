using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        IChatRepository Chat
        {
            get;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
         Task SaveAsync();
    }
}
