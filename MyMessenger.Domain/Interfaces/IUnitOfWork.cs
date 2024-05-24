namespace MyMessenger.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        IChatRepository Chat
        {
            get;
        }
        IMessageRepository Message
        {
            get;
        }
        IUserRepository User
        {
            get;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
         Task SaveAsync();
    }
}
