using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using System.Data.Entity;

namespace MyMessenger.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }
        public void UpdateImage(string userId, string imageurl)
        {
            dbContext.Set<User>().FirstOrDefault(x => x.Id == userId).Image = imageurl;
        }
        public User GetById(string userId)
        {
            return dbContext.Set<User>().FirstOrDefault(x => x.Id == userId);
        }
    }
}
