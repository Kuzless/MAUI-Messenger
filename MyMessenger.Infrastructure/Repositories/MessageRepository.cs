using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Domain.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context) { }
        public async Task Add(string userId, int chatId, string text)
        {
            dbContext.Set<Message>().Add(new Message { UserId = userId, ChatId = chatId, Text = text, DateTime = DateTime.Now });
        }
        public async Task Update(string userId, int chatId, string text, DateTime time)
        {
            var message = dbContext.Set<Message>().FirstOrDefault(m => m.UserId == userId && m.ChatId == chatId && m.Text == text && m.DateTime == time);
            message.Text = text;
            dbContext.Set<Message>().Update(message);
        }
        public async Task Delete(string userId, int chatId, string text, DateTime time)
        {
            var message = dbContext.Set<Message>().FirstOrDefault(m => m.UserId == userId && m.ChatId == chatId && m.Text == text && m.DateTime == time && m.Text == text);
            throw new NotImplementedException();
        }
    }
}
