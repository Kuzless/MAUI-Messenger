using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext context;
        private readonly Dictionary<Type, object> repos = new Dictionary<Type, object>();
        private bool disposed;
        public IChatRepository Chat { get; private set; }
        public IMessageRepository Message { get; private set; }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
            Chat = new ChatRepository(context);
            Message = new MessageRepository(context);
            repos[typeof(Entities.Chat)] = Chat;
            repos[typeof(Entities.Message)] = Message;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!repos.Keys.Contains(type))
            {
                repos[type] = new GenericRepository<TEntity>(context);
            }
            return (IGenericRepository<TEntity>)repos[type];
        }

        public async Task SaveAsync()
        {   
            await context.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
