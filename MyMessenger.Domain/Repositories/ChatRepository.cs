using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Chat> GetChatById(int id)
        {
            return await dbContext.Set<Chat>().Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddChat(string chatName, string ownerId, User user)
        {
            var chat = new Chat { Name = chatName, OwnerId = ownerId };
            chat.Users.Add(user);
            await dbContext.Set<Chat>().AddAsync(chat);
        }
        public void AddMember(Chat chat, User user)
        {
            chat.Users.Add(user);
        }
        public void DeleteMember(Chat chat, User user)
        {
            chat.Users.Remove(user);
        }
    }
}
