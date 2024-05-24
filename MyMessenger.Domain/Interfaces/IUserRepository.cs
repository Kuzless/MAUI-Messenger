using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        void UpdateImage(string userId, string imageurl);
        User GetById(string userId);
    }
}
