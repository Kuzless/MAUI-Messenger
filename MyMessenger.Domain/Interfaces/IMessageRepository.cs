using MyMessenger.Domain.Entities;

namespace MyMessenger.Domain.Interfaces
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task Add(string userId, int chatId, string text);
        Task Update(string userId, int chatId, string text, DateTime time);
        Task Delete(string userId, int chatId, string text, DateTime time);
    }
}
