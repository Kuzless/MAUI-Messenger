using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MyMessenger.Domain.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DatabaseContext context) : base(context) { }
        public async Task<Message> AddMessage(Message message)
        {
            await dbContext.Set<Message>().AddAsync(message);
            return message;
        }
        public IQueryable<Message> GetMessagesByChatId(int chatId)
        {
            return dbContext.Set<Message>().Include(m => m.User).Where(m => m.ChatId == chatId);
        }
        public async Task<Message> GetMessageById(int id)
        {
            return await dbContext.Set<Message>().Include(m => m.User).Where(m => m.Id == id).FirstAsync();
        }
    }
}
