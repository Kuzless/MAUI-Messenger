using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Domain.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Message>> GetAll()
        {
            throw new NotImplementedException();
        }
        public async Task Add()
        {
            throw new NotImplementedException();
        }
        public async Task Update()
        {
            throw new NotImplementedException();
        }
        public async Task Delete()
        {
            throw new NotImplementedException();
        }
    }
}
