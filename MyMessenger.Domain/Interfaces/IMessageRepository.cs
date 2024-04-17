using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task AddMessage(string userId, int chatId, string text);
        IQueryable<Message> GetMessagesByChatId(int chatId);
    }
}
