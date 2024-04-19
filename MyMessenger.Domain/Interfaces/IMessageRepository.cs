using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<Message> AddMessage(Message message);
        IQueryable<Message> GetMessagesByChatId(int chatId);
        Message GetMessageById(int id);
    }
}
