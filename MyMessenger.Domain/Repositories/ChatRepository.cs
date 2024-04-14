using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using System;
using System.Data.Entity;

namespace MyMessenger.Domain.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(DatabaseContext context) : base(context) { }
        public IQueryable<Chat> GetChatsByUserId(string userId)
        {
            var chats = dbContext.Set<Chat>().Where(chat => chat.Users.Any(user => user.Id == userId));
            return chats;
        }
        public Chat GetChatById(int id)
        {
            return dbContext.Set<Chat>().Include(x => x.Users).FirstOrDefault(x => x.Id == id);
        }
        public async Task AddChat(string chatName, string ownerId, User user)
        {
            var chat = new Chat { Name = chatName, OwnerId = ownerId };
            chat.Users.Add(user);
            await dbContext.Set<Chat>().AddAsync(chat);
        }
        public async Task AddMember(Chat chat, User user)
        {
            chat.Users.Add(user);
        }
        public async Task DeleteMember(Chat chat, User user)
        {
            chat.Users.Remove(user);
        }
    }
}
