using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context) { }
        public Task Add(Chat chat)
        {
            throw new NotImplementedException();
        }
    }
}
