using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Domain.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(DatabaseContext context) : base(context) { }
        public Chat GetChatByNameOwner(string chatName, string ownerId)
        {
            var chat = dbContext.Set<Chat>().FirstOrDefault(c => c.Name == chatName && c.OwnerId == ownerId);
            return chat;
        }
        public IQueryable<Chat> GetChatsByUserId(string userId)
        {
            var chats = dbContext.Set<Chat>().Where(chat => chat.Users.Any(user => user.Id == userId));
            return chats;
        }
        public async Task AddChat(string chatName, string ownerId)
        {
            dbContext.Set<Chat>().Add(new Chat { Name = chatName, OwnerId = ownerId });
        }
        public async Task DeleteChat(Chat chat)
        {
            dbContext.Set<Chat>().Remove(chat);
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
