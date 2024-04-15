using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Domain.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context) { }
        public async Task AddMessage(string userId, int chatId, string text)
        {
            var message = new Message() { UserId = userId, ChatId = chatId, Text = text, DateTime = DateTime.Now };
            await dbContext.Set<Message>().AddAsync(message);
        }
    }
}
