using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Domain.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        protected readonly DatabaseContext dbContext;
        public ChatRepository(DatabaseContext context) : base(context) 
        {
            dbContext = context;
        }
        public IQueryable<Chat> GetChatsByUserId(string userId)
        {
            var chats = dbContext.Set<Chat>().Where(chat => chat.Users.Any(user => user.Id == userId));
            return chats;
        }
        public async Task AddChat(string chatName, string userId)
        {
            var chat = dbContext.Chats.FirstOrDefault(c => c.Name == chatName);
        }
        public async Task AddMember(Chat chat)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteChat()
        {
            throw new NotImplementedException();
        }
        public async Task DeleteMember()
        {
            throw new NotImplementedException();
        }
    }
}
