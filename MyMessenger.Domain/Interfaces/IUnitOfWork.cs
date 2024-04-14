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
